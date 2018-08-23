using Lykke.Service.LykkeDevelopers.Client.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.Client.Api
{
    /// <summary>
    /// Teams API
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public interface ITeamApi
    {
        /// <summary>
        /// Gets all teams
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [Get("/Api/Team/GetTeams")]
        Task<List<TeamModel>> GetTeams();


        /// <summary>
        /// Gets team by ID
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns></returns>
        [Get("/Api/Team/GetTeam/{teamID}")]
        Task<TeamModel> GetTeam(string teamID);

        /// <summary>
        /// Save team
        /// </summary>
        /// <param team="team">Team Model</param>
        /// <returns></returns>
        [Get("/Api/Team/SaveTeam")]
        Task<bool> SaveTeam(TeamModel team);

        /// <summary>
        /// Remove team by ID
        /// </summary>
        /// <param name="teamId">devID</param>
        /// <returns></returns>
        [Get("/Api/Team/RemoveTeam")]
        Task<bool> RemoveTeam(string teamId);
    }
}
