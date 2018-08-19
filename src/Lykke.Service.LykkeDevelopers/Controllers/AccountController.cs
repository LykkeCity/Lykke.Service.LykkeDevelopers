using Lykke.Service.LykkeDevelopers.AzureRepositories.User;
using Lykke.Service.LykkeDevelopers.Core.Domain.User;
using Lykke.Service.LykkeDevelopers.Extentions;
using Lykke.Service.LykkeDevelopers.Models;
using Lykke.Service.LykkeDevelopers.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : Controller
    {
        private string HomeUrl => Url.Action("Developers", "Home");
        private readonly AppSettings _appSettings;
        private readonly IUserRepository _userRepository;

        private string ApiClientId { get; }
        private string AvailableEmailsRegex { get; }

        public AccountController(
            AppSettings appSettings,
            IUserRepository userRepository) 
        {
            _appSettings = appSettings;
            _userRepository = userRepository;

            ApiClientId = _appSettings.LykkeDevelopersService.ApiClientId;
            AvailableEmailsRegex = _appSettings.LykkeDevelopersService.AvailableEmailsRegex;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> SignIn(string returnUrl)
        {
            try
            {
                if ((await _userRepository.GetUsers()).Count == 0)
                {
                    var usr = new UserEntity
                    {
                        PasswordHash = _appSettings.LykkeDevelopersService.DefaultPassword.GetHash(),
                        RowKey = _appSettings.LykkeDevelopersService.DefaultUserEmail,
                        FirstName = _appSettings.LykkeDevelopersService.DefaultUserFirstName,
                        LastName = _appSettings.LykkeDevelopersService.DefaultUserLasttName,
                        Active = true,
                        //Admin = true
                    };

                    await _userRepository.SaveUser(usr);
                }

                ViewData["ReturnUrl"] = returnUrl;

                return View(new SignInModel());
            }
            catch (Exception ex)
            {
                return View(new SignInModel ());
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn(string email, string password, string returnUrl)
        {
            try
            {
                var user = await _userRepository.GetUserByUserEmail(email);

                //if (user == null)
                //    return View(new SignInModel());

                // temporary commented
                if (user == null)
                {
                    if (!String.IsNullOrWhiteSpace(_appSettings.LykkeDevelopersService.DefaultUserEmail) && !String.IsNullOrWhiteSpace(_appSettings.LykkeDevelopersService.DefaultPassword))
                    {
                        user = new UserEntity()
                        {
                            Active = true,
                            //Admin = true,
                            FirstName = _appSettings.LykkeDevelopersService.DefaultUserFirstName,
                            LastName = _appSettings.LykkeDevelopersService.DefaultUserLasttName,
                            Salt = String.Empty,
                            PasswordHash = $"{_appSettings.LykkeDevelopersService.DefaultPassword}{String.Empty}".GetHash(),
                            PartitionKey = "U",
                            RowKey = _appSettings.LykkeDevelopersService.DefaultUserEmail
                        };
                    }
                    else
                    {
                        return View(new SignInModel());
                    }
                }

                var passwordHash = $"{password}{user.Salt}".GetHash();
                if (user.PasswordHash != passwordHash)
                    user = null;

                if (user != null && user.Active.HasValue && user.Active.Value)
                {
                    var claims = new List<Claim>
                 {
                     new Claim(ClaimTypes.Sid, email),
                     //new Claim("IsAdmin", user.Admin.ToString()),
                     new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}".Trim())
                 };

                    var claimsIdentity = new ClaimsIdentity(claims, "password");
                    var claimsPrinciple = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrinciple);

                    return Redirect(Url.IsLocalUrl(returnUrl) ? returnUrl : "~/");
                }

                return View(new SignInModel ());
            }
            catch (Exception ex)
            {
                return View(new SignInModel ());
            }
        }

        
        [AllowAnonymous]
        [Route("Account/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Route("Account/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Route("Account/ChangePassword")]
        public async Task<IActionResult> ChangePassword(string oldPassword, string password)
        {
            try
            {
                var user = await _userRepository.GetUserByUserEmail(Request.HttpContext.GetUserEmail() ?? "anonymous", oldPassword.GetHash());

                if (user == null)
                {
                    ViewData["incorrectPassword"] = true;
                    return View();
                }

                byte[] salt = new byte[128 / 8];

                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                user.Salt = Convert.ToBase64String(salt);
                user.PasswordHash = $"{password}{user.Salt}".GetHash();
                await _userRepository.SaveUser(user);

                return Redirect(HomeUrl);
            }
            catch (Exception ex)
            {
                return Redirect(HomeUrl);
            }
        }

        [Route("Account/SignOut")]
        public async Task<IActionResult> SignOut()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Redirect(HomeUrl);
            }
            catch (Exception ex)
            {
                return Redirect(HomeUrl);
            }
        }    
    }
}
