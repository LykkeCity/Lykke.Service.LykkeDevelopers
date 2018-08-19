using Lykke.Service.LykkeDevelopers.Core.Domain.Team;
using Lykke.Service.LykkeDevelopers.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.Services
{
    public class TeamsService : ITeamsService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamsService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public Task<ITeamEntity> GetTeamAsync(string rowKey)
        {
            return _teamRepository.GetTeamAsync(rowKey);
        }

        public Task<List<ITeamEntity>> GetTeamsAsync()
        {
            return _teamRepository.GetTeams();
        }

        public Task<bool> RemoveTeamAsync(string rowKey)
        {
            return _teamRepository.RemoveTeam(rowKey);
        }

        public Task<bool> SaveTeamAsync(ITeamEntity team)
        {
            return _teamRepository.SaveTeam(team);
        }
    }
}
