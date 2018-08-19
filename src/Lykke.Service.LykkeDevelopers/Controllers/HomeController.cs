using Lykke.Service.LykkeDevelopers.AzureRepositories.Developer;
using Lykke.Service.LykkeDevelopers.AzureRepositories.Team;
using Lykke.Service.LykkeDevelopers.Core.Developer;
using Lykke.Service.LykkeDevelopers.Core.Team;
using Lykke.Service.LykkeDevelopers.Extentions;
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
    [Authorize]
    //[Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly IDeveloperRepository _developerRepository;
        private readonly ITeamRepository _teamRepository;


        public HomeController(
            AppSettings appSettings,
            IDeveloperRepository developerRepository,
            ITeamRepository teamRepository)
        {
            _appSettings = appSettings;
            _developerRepository = developerRepository;
            _teamRepository = teamRepository;

        }

        [Route("Home/Developers")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Developers()
        {
            var devs = await GetAllDevs();
            var teams = await GetAllTeams();
            return View(new DevelopersModel { Developers = devs, Teams = teams });
        }

        public async Task<IActionResult> Teams()
        {
            var teams = await GetAllTeams();            
            return View(new TeamsModel { Teams = teams });
        }

        [HttpPost]
        [Route("Home/SaveDeveloper")]
        public async Task<IActionResult> SaveDeveloper(DeveloperModel dev)
        {
            try
            {
                var checkDev = await GetAllDevs();
                if (String.IsNullOrWhiteSpace(dev.TelegramAcc))
                {
                    dev.TelegramAcc = "";
                }

                if (String.IsNullOrWhiteSpace(dev.GithubAcc))
                {
                    dev.GithubAcc = "";
                }

                foreach (var devToCheck in checkDev)
                {
                    if(devToCheck.RowKey != dev.RowKey)
                    {
                        if (!String.IsNullOrWhiteSpace(devToCheck.Email) && devToCheck.Email.ToLower() == dev.Email.ToLower())
                        {
                            return new JsonResult(new { Result = ResultCode.EmailAlreadyExists });
                        }
                        else if (!String.IsNullOrWhiteSpace(devToCheck.TelegramAcc) && devToCheck.TelegramAcc.ToLower() == dev.TelegramAcc.ToLower())
                        {
                            return new JsonResult(new { Result = ResultCode.TelegramAlreadyExists });
                        }
                        else if (!String.IsNullOrWhiteSpace(devToCheck.GithubAcc) && devToCheck.GithubAcc.ToLower() == dev.GithubAcc.ToLower())
                        {
                            return new JsonResult(new { Result = ResultCode.GitAlreadyExists });
                        }
                    }                    
                }
                var devToSave = (await _developerRepository.GetDevAsync(dev.RowKey)) as DeveloperEntity ?? new DeveloperEntity();

                devToSave.Email = dev.Email;
                devToSave.FirstName = dev.FirstName;
                devToSave.LastName = dev.LastName;
                devToSave.TelegramAcc = dev.TelegramAcc;
                devToSave.GithubAcc = dev.GithubAcc;
                devToSave.Team = dev.Team;

                await _developerRepository.SaveDeveloper(devToSave);
                var result = await GetAllDevs();
                var teams = await GetAllTeams();
                return new JsonResult(new { Result = ResultCode.Ok, Json = JsonConvert.SerializeObject(result), Teams = JsonConvert.SerializeObject(teams) });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new JsonResult(new { });
            }
        }

        [HttpPost]
        [Route("Home/SaveTeam")]
        public async Task<IActionResult> SaveTeam(TeamModel team)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(team.Name))
                {
                    return new JsonResult(new { Result = ResultCode.InvalidInput });
                }

                var checkTeam = await GetAllTeams();
                foreach (var teamToCheck in checkTeam)
                {
                    if (teamToCheck.RowKey != team.RowKey)
                    {
                        if (teamToCheck.Name.ToLower() == team.Name.ToLower())
                        {
                            return new JsonResult(new { Result = ResultCode.TeamAlreadyExists });
                        }
                    }
                }

                var teamToSave = (await _teamRepository.GetTeamAsync(team.RowKey)) as TeamEntity ?? new TeamEntity();

                //Renaming check wen need to rename team on alll developers
                if (!String.IsNullOrWhiteSpace(teamToSave.RowKey) && team.Name != teamToSave.Name)
                {
                    var devs = await GetAllDevs();
                    foreach(var dev in devs)
                    {
                        if(dev.Team == teamToSave.Name)
                        {
                            dev.Team = team.Name;
                            var devToSave = new DeveloperEntity();
                            devToSave.RowKey = dev.RowKey;
                            devToSave.Email = dev.Email;
                            devToSave.FirstName = dev.FirstName;
                            devToSave.LastName = dev.LastName;
                            devToSave.TelegramAcc = dev.TelegramAcc;
                            devToSave.GithubAcc = dev.GithubAcc;
                            devToSave.Team = dev.Team;
                            await _developerRepository.SaveDeveloper(devToSave);
                        }
                    }
                }

                teamToSave.Name = team.Name;

                await _teamRepository.SaveTeam(teamToSave);
                var result = await GetAllTeams();

                return new JsonResult(new { Result = ResultCode.Ok, Json = JsonConvert.SerializeObject(result) });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new JsonResult(new { });
            }
        }

        [HttpPost]
        [Route("Home/RemoveDeveloper")]
        public async Task<IActionResult> RemoveDeveloper(string RowKey)
        {
            try
            {
                await _developerRepository.RemoveDeveloper(RowKey);
                var result = await GetAllDevs();
                var teams = await GetAllTeams();
                return new JsonResult(new { Result = ResultCode.Ok, Json = JsonConvert.SerializeObject(result), Teams = JsonConvert.SerializeObject(teams) });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new JsonResult(new { });
            }
        }

        [HttpPost]
        [Route("Home/RemoveTeam")]
        public async Task<IActionResult> RemoveTeam(string RowKey)
        {
            try
            {
                var teamToRemove = await _teamRepository.GetTeamAsync(RowKey);
                var devs = await GetAllDevs();
                foreach (var dev in devs)
                {
                    if (dev.Team == teamToRemove.Name)
                    {
                        dev.Team = "Team was removed";
                        var devToSave = new DeveloperEntity();
                        devToSave.RowKey = dev.RowKey;
                        devToSave.Email = dev.Email;
                        devToSave.FirstName = dev.FirstName;
                        devToSave.LastName = dev.LastName;
                        devToSave.TelegramAcc = dev.TelegramAcc;
                        devToSave.GithubAcc = dev.GithubAcc;
                        devToSave.Team = dev.Team;
                        await _developerRepository.SaveDeveloper(devToSave);
                    }
                }

                await _teamRepository.RemoveTeam(RowKey);

                var result = await GetAllTeams();
                return new JsonResult(new { Json = JsonConvert.SerializeObject(result) });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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

        private async Task<List<TeamModel>> GetAllTeams()
        {
            var result = await _teamRepository.GetTeams();

            var teams = (from t in result
                        let team = t as TeamEntity
                        //orderby dev.Email
                        select new TeamModel
                        {
                            RowKey = team.RowKey,
                            Name = team.Name,

                        }).ToList();
            return teams;
        }
    }
}
