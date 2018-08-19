using Lykke.Common.Api.Contract.Responses;
using Lykke.Service.LykkeDevelopers.AzureRepositories.Developer;
using Lykke.Service.LykkeDevelopers.Client.Models;
using Lykke.Service.LykkeDevelopers.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.LykkeDevelopers.Core.Domain.Developer;
using Lykke.Service.LykkeDevelopers.Extentions;
using System;

namespace Lykke.Service.LykkeDevelopers.Controllers
{
    [Route("Api/Developer")]
    public class DeveloperController : Controller
    {
        private readonly IDevelopersService _developersService;

        public DeveloperController(IDevelopersService developersService)
        {
            _developersService = developersService;
        }

        /// <summary>
        /// Gets all developers
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDevelopers")]
        [SwaggerOperation("GetDevelopers")]
        [ProducesResponseType(typeof(List<DeveloperModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<List<DeveloperModel>> GetDevelopers()
        {
            var devs = await _developersService.GetDevelopersAsync();
            return Mapper.Map<List<DeveloperModel>>(devs);
        }


        /// <summary>
        /// Gets developer by ID
        /// </summary>
        /// <param name="devID">devID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDeveloper/{devID}")]
        [SwaggerOperation("GetDeveloper")]
        [ProducesResponseType(typeof(DeveloperModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<DeveloperModel> GetDeveloper(string devID)
        {
            var dev = await _developersService.GetDeveloperAsync(devID);
            return Mapper.Map<DeveloperModel>(dev);
        }


        /// <summary>
        /// Gets all developers in team
        /// </summary>
        /// <param name="teamName">teamName</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDevelopersByTeam/{teamName}")]
        [SwaggerOperation("GetDevelopersByTeam")]
        [ProducesResponseType(typeof(List<DeveloperModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<List<DeveloperModel>> GetDevelopersByTeam(string teamName)
        {
            var devs = await _developersService.GetDevelopersByTeamAsync(teamName);
            return Mapper.Map<List<DeveloperModel>>(devs);
        }

        /// <summary>
        /// Gets developers team by telegram account
        /// </summary>
        /// <param name="telegramAcc">telegramAcc</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDeveloperTeam/{telegramAcc}")]
        [SwaggerOperation("GetDeveloperTeam")]
        [ProducesResponseType(typeof(TeamModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<TeamModel> GetDeveloperTeam(string telegramAcc)
        {
            var team = await _developersService.GetDeveloperTeamAsync(telegramAcc);
            return Mapper.Map<TeamModel>(team);
        }

        /// <summary>
        /// If developer in this team returns true
        /// </summary>
        /// <param name="telegramAcc">telegramAcc</param>
        /// <param name="teamName">teamName</param>
        /// <returns></returns>
        [HttpGet]
        [Route("IsDeveloperInTeamAsync/{telegramAcc}/{teamName}")]
        [SwaggerOperation("IsDeveloperInTeamAsync")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<bool> IsDeveloperInTeamAsync(string telegramAcc, string teamName)
        {            
            return await _developersService.IsDeveloperInTeamAsync(telegramAcc, teamName); 
        }

        /// <summary>
        /// Remove developer by ID
        /// </summary>
        /// <param name="devID">devID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("RemoveDeveloper")]
        [SwaggerOperation("RemoveDeveloper")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<bool> RemoveDeveloper(string devID)
        {
            return await _developersService.RemoveDeveloperAsync(devID);
        }

        /// <summary>
        /// Save developer
        /// </summary>
        /// <param name="developer">DeveloperModel</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveDeveloper")]
        [SwaggerOperation("SaveDeveloper")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<bool> SaveDeveloper(DeveloperModel developer)
        {
            var checkDev = await GetDevelopers();
            if (String.IsNullOrWhiteSpace(developer.TelegramAcc))
            {
                developer.TelegramAcc = "";
            }

            if (String.IsNullOrWhiteSpace(developer.GithubAcc))
            {
                developer.GithubAcc = "";
            }

            foreach (var devToCheck in checkDev)
            {
                if (devToCheck.RowKey != developer.RowKey)
                {
                    if (!String.IsNullOrWhiteSpace(devToCheck.Email) && devToCheck.Email.ToLower() == developer.Email.ToLower())
                    {
                        return false;
                    }
                    else if (!String.IsNullOrWhiteSpace(devToCheck.TelegramAcc) && devToCheck.TelegramAcc.ToLower() == developer.TelegramAcc.ToLower())
                    {
                        return false;
                    }
                    else if (!String.IsNullOrWhiteSpace(devToCheck.GithubAcc) && devToCheck.GithubAcc.ToLower() == developer.GithubAcc.ToLower())
                    {
                        return false;
                    }
                }
            }
            var devToSave = (await _developersService.GetDeveloperAsync(developer.RowKey)) as DeveloperEntity ?? new DeveloperEntity();

            devToSave.Email = developer.Email;
            devToSave.FirstName = developer.FirstName;
            devToSave.LastName = developer.LastName;
            devToSave.TelegramAcc = developer.TelegramAcc;
            devToSave.GithubAcc = developer.GithubAcc;
            devToSave.Team = developer.Team;

            return await _developersService.SaveDeveloperAsync(devToSave);
        }
    }
}
