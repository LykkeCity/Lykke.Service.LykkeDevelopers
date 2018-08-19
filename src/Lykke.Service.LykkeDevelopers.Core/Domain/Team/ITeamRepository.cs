using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.Core.Domain.Team
{
    public interface ITeamRepository
    {
        /// <summary>
        /// Get team by ID
        /// </summary>
        /// <param name=teamId">RowKey</param>
        /// <returns></returns>
        Task<ITeamEntity> GetTeamAsync(string rowKey);
        /// <summary>
        /// Save team
        /// </summary>
        /// <param name="team">Team</param>
        /// <returns></returns>
        Task<bool> SaveTeam(ITeamEntity team);
        /// <summary>
        /// Get list of all teams
        /// </summary>
        /// <returns></returns>
        Task<List<ITeamEntity>> GetTeams();
        /// <summary>
        /// Remove team by ID
        /// </summary>
        /// <param name="teamId">RowKey</param>
        /// <returns></returns>
        Task<bool> RemoveTeam(string rowKey);
    }
}
