using AutoMapper;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Service.LykkeDevelopers.AzureRepositories.Developer;
using Lykke.Service.LykkeDevelopers.AzureRepositories.Team;
using Lykke.Service.LykkeDevelopers.Client.Models;
using Lykke.Service.LykkeDevelopers.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.Controllers
{
    [Route("Api/Team")]
    public class TeamController : Controller
    {
        private readonly IDevelopersService _developersService;
        private readonly ITeamsService _teamsService;


        public TeamController(
            IDevelopersService developersService,
            ITeamsService teamsService)
        {
            _developersService = developersService;
            _teamsService = teamsService;
        }

        /// <summary>
        /// Gets all teams
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTeams")]
        [SwaggerOperation("GetTeams")]
        [ProducesResponseType(typeof(List<TeamModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<List<TeamModel>> GetTeams()
        {
            var teams = await _teamsService.GetTeamsAsync();
            return Mapper.Map<List<TeamModel>>(teams);
        }


        /// <summary>
        /// Gets team by ID
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTeam/{teamID}")]
        [SwaggerOperation("GetTeam")]
        [ProducesResponseType(typeof(TeamModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<TeamModel> GetTeam(string teamID)
        {
            var team = await _teamsService.GetTeamAsync(teamID);
            return Mapper.Map<TeamModel>(team);
        }

        /// <summary>
        /// Save team
        /// </summary>
        /// <param team="team">Team Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveTeam")]
        [SwaggerOperation("SaveTeam")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<bool> SaveTeam(TeamModel team)
        {
            if (String.IsNullOrWhiteSpace(team.Name))
            {
                return false;
            }

            var checkTeam = await GetTeams();
            foreach (var teamToCheck in checkTeam)
            {
                if (teamToCheck.RowKey != team.RowKey)
                {
                    if (teamToCheck.Name.ToLower() == team.Name.ToLower())
                    {
                        return false;
                    }
                }
            }

            var teamToSave = (await _teamsService.GetTeamAsync(team.RowKey)) as TeamEntity ?? new TeamEntity();

            //Renaming check wen need to rename team on alll developers
            if (!String.IsNullOrWhiteSpace(teamToSave.RowKey) && team.Name != teamToSave.Name)
            {
                var devs = await _developersService.GetDevelopersAsync();
                foreach (var dev in devs)
                {
                    if (dev.Team == teamToSave.Name)
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
                        await _developersService.SaveDeveloperAsync(devToSave);
                    }
                }
            }

            teamToSave.Name = team.Name;

            return await _teamsService.SaveTeamAsync(teamToSave);
        }

        /// <summary>
        /// Remove team by ID
        /// </summary>
        /// <param name="teamId">devID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("RemoveTeam")]
        [SwaggerOperation("RemoveTeam")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<bool> RemoveTeam(string teamId)
        {
            try
            {
                var teamToRemove = await _teamsService.GetTeamAsync(teamId);
                if (teamToRemove == null)
                    return false;
                var devs = await _developersService.GetDevelopersAsync();
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
                        await _developersService.SaveDeveloperAsync(devToSave);
                    }
                }
                return await _teamsService.RemoveTeamAsync(teamId);
            }
            catch
            {
                return false;
            }            
        }
    }
}
