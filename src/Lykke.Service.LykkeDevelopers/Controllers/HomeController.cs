using Lykke.Service.LykkeDevelopers.AzureRepositories.Developer;
using Lykke.Service.LykkeDevelopers.Contract.Models;
using Lykke.Service.LykkeDevelopers.Core.Developer;
using Lykke.Service.LykkeDevelopers.Models;
using Lykke.Service.LykkeDevelopers.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.Controllers
{
    //[Authorize]
    [Route("/api/[controller]")]
    public class HomeController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly IDeveloperRepository _developerRepository;


        public HomeController(
            AppSettings appSettings,
            IDeveloperRepository developerRepository)
        {
            _appSettings = appSettings;
            _developerRepository = developerRepository;

        }

        [HttpGet]
        [Route("{id}")]
        public async Task<string> Test(string id)
        {
            return !String.IsNullOrEmpty(id) ? id : "TestNullValue";
        }

        [HttpGet]
        public async Task<IActionResult> Developers()
        {
            var devs = await GetAllDevs();

            foreach (var r in devs)
            {
                Console.WriteLine(r.Email);
            }

            return Ok(devs);
        }

        [HttpPost]
        [Route("Home/SaveDeveloper")]
        public async Task<IActionResult> SaveDeveloper(DeveloperModel dev)
        {
            try
            {
                var devToSave = (await _developerRepository.GetDevAsync(dev.RowKey)) as DeveloperEntity ?? new DeveloperEntity();

                devToSave.Email = dev.Email;
                devToSave.FirstName = dev.FirstName;
                devToSave.LastName = dev.LastName;
                devToSave.TelegramAcc = dev.TelegramAcc;
                devToSave.GithubAcc = dev.GithubAcc;
                devToSave.Team = dev.Team;

                Console.WriteLine(devToSave.Email);

                await _developerRepository.SaveDeveloper(devToSave);
                var result = await GetAllDevs();

                Console.WriteLine(JsonConvert.SerializeObject(result));

                return new JsonResult(new { Json = JsonConvert.SerializeObject(result) });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { });
            }
        }

        private async Task<List<DeveloperModel>> GetAllDevs()
        {
            var result = await _developerRepository.GetDevelopers();

            var devs = (from d in result
                        let dev = d as DeveloperEntity
                        //orderby dev.Email
                        select new DeveloperModel
                        {
                            RowKey = dev.RowKey,
                            Email = dev.Email,
                            FirstName = dev.FirstName,
                            LastName = dev.LastName,
                            GithubAcc = dev.GithubAcc,
                            Team = dev.Team,
                            TelegramAcc = dev.TelegramAcc,
                        }).ToList();
            return devs;
        }
    }
}
