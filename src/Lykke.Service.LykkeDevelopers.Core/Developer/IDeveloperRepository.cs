using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.Core.Developer
{
    public interface IDeveloperRepository
    {
        /// <summary>
        /// Get developer by ID
        /// </summary>
        /// <param name=devId">RowKey</param>
        /// <returns></returns>
        Task<IDeveloperEntity> GetDevAsync(string rowKey);
        /// <summary>
        /// Save developer
        /// </summary>
        /// <param name="developer">Developer</param>
        /// <returns></returns>
        Task<bool> SaveDeveloper(IDeveloperEntity developer);
        /// <summary>
        /// Get list of all developers
        /// </summary>
        /// <returns></returns>
        Task<List<IDeveloperEntity>> GetDevelopers();
        /// <summary>
        /// Remove developer by ID
        /// </summary>
        /// <param name="devId">RowKey</param>
        /// <returns></returns>
        Task<bool> RemoveDeveloper(string rowKey);
    }
}
