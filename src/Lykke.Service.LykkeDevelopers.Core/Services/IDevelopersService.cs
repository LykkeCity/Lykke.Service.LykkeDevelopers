using Lykke.Service.LykkeDevelopers.Core.Domain.Developer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.Core.Services
{
    public interface IDevelopersService
    {
        Task<IDeveloperEntity> GetDevAsync(string rowKey);
        Task<bool> SaveDeveloper(IDeveloperEntity developer);
        Task<List<IDeveloperEntity>> GetDevelopers();
        Task<bool> RemoveDeveloper(string rowKey);
    }
}
