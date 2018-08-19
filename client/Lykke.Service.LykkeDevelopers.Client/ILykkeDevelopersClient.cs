using Lykke.Service.LykkeDevelopers.Contract.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.Client
{
    /// <summary>
    /// DevelopersClient interface
    /// </summary>
    public interface ILykkeDevelopersClient
    {
        /// <summary>
        /// returns all developers
        /// </summary>
        /// <returns>returns a list of developers</returns>
        Task<List<DeveloperModel>> GetDevelopersAsync();

        /// <summary>
        /// creates new developer
        /// </summary>
        /// <param name="model">The model that describes an developer</param>
        /// <returns>returns list of developers</returns>
        Task<List<DeveloperModel>> CreateDeveloperAsync(DeveloperModel model);
        
        Task<string> Test(string id);
    }
}
