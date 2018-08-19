using Lykke.Service.LykkeDevelopers.Core.Domain.Team;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.Core.Services
{
    public interface ITeamsService
    {
        Task<ITeamEntity> GetTeamAsync(string rowKey);
        Task<bool> SaveTeamAsync(ITeamEntity team);
        Task<List<ITeamEntity>> GetTeamsAsync();
        Task<bool> RemoveTeamAsync(string rowKey);
    }
}
