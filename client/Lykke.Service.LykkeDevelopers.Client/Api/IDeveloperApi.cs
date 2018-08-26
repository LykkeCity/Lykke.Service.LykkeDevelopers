using Lykke.Service.LykkeDevelopers.Client.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.Client.Api
{
    /// <summary>
    /// Developers API
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public interface IDeveloperApi
    {
        /// <summary>
        /// Gets all developers
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [Get("/Api/Developer/GetDevelopers")]
        Task<List<DeveloperModel>> GetDevelopers();


        /// <summary>
        /// Gets developer by ID
        /// </summary>
        /// <param name="devID">devID</param>
        /// <returns></returns>
        [Get("/Api/Developer/GetDeveloper/{devID}")]
        Task<DeveloperModel> GetDeveloper(string devID);

        /// <summary>
        /// Gets all developers in team
        /// </summary>
        /// <param name="teamName">teamName</param>
        /// <returns></returns>
        [Get("/Api/Developer/GetDevelopersByTeam/{teamName}")]
        Task<List<DeveloperModel>> GetDevelopersByTeam(string teamName);

        /// <summary>
        /// Gets developers team by telegram account
        /// </summary>
        /// <param name="telegramAcc">telegramAcc</param>
        /// <returns></returns>
        [Get("/Api/Developer/GetDeveloperTeam/{telegramAcc}")]
        Task<TeamModel> GetDeveloperTeam(string telegramAcc);

        /// <summary>
        /// If developer in this team returns true
        /// </summary>
        /// <param name="telegramAcc">telegramAcc</param>
        /// <param name="teamName">teamName</param>
        /// <returns></returns>
        [Get("/Api/Developer/IsDeveloperInTeamAsync/{telegramAcc}/{teamName}")]
        Task<bool> IsDeveloperInTeam(string telegramAcc, string teamName);

        /// <summary>
        /// Remove developer by ID
        /// </summary>
        /// <param name="devID">devID</param>
        /// <returns></returns>
        [Post("/Api/Developer/RemoveDeveloper")]
        Task<bool> RemoveDeveloper(string devID);

        /// <summary>
        /// Save developer
        /// </summary>
        /// <param name="developer">DeveloperModel</param>
        /// <returns></returns>
        [Post("/Api/Developer/SaveDeveloper")]
        Task<bool> SaveDeveloper([Body] DeveloperModel developer);
    }
}
