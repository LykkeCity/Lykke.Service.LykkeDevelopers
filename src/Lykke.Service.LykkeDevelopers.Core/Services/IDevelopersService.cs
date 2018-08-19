using Lykke.Service.LykkeDevelopers.Core.Domain.Developer;
using Lykke.Service.LykkeDevelopers.Core.Domain.Team;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.Core.Services
{
    public interface IDevelopersService
    {
        Task<IDeveloperEntity> GetDeveloperAsync(string rowKey);
        Task<bool> SaveDeveloperAsync(IDeveloperEntity developer);
        Task<List<IDeveloperEntity>> GetDevelopersAsync();
        Task<bool> RemoveDeveloperAsync(string rowKey);
        Task<bool> IsDeveloperInTeamAsync(string telegramAcc, string teamName);
        Task<ITeamEntity> GetDeveloperTeamAsync(string telegramAcc);
        Task<List<IDeveloperEntity>> GetDevelopersByTeamAsync(string teamName);
    }
}
