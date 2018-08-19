using Lykke.Service.LykkeDevelopers.Contract.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.Client.Api
{
    internal interface IHomeApi
    {
        /// <summary>
        /// get all developers
        /// </summary>
        /// <returns></returns>
        [Get("/api/home")]
        Task<List<DeveloperModel>> GetAllDevs();

        /// <summary>
        /// create new developer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Post("/api/home")]
        Task<List<DeveloperModel>> CreateAsync(DeveloperModel model);

        [Get("api/home/{id}")]
        Task<string> Test(string id);
    }
}
