using Lykke.Service.LykkeDevelopers.Core.Domain.Team;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.Core.Services
{
    public interface ITeamsService
    {
        Task<ITeamEntity> GetTeamAsync(string rowKey);
        Task<bool> SaveTeam(ITeamEntity team);
        Task<List<ITeamEntity>> GetTeams();
        Task<bool> RemoveTeam(string rowKey);
    }
}
