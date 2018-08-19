using Lykke.Service.LykkeDevelopers.Core.Domain.Developer;
using Lykke.Service.LykkeDevelopers.Core.Domain.Team;
using Lykke.Service.LykkeDevelopers.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Lykke.Service.LykkeDevelopers.Services
{
    public class DevelopersService : IDevelopersService
    {
        private readonly IDeveloperRepository _developerRepository;
        private readonly ITeamRepository _teamRepository;

        public DevelopersService(
            IDeveloperRepository developerRepository,
            ITeamRepository teamRepository)
        {
            _developerRepository = developerRepository;
            _teamRepository = teamRepository;
        }

        public Task<IDeveloperEntity> GetDeveloperAsync(string rowKey)
        {
            return _developerRepository.GetDevAsync(rowKey);
        }

        public Task<List<IDeveloperEntity>> GetDevelopersAsync()
        {
            return _developerRepository.GetDevelopers();
        }

        public async Task<List<IDeveloperEntity>> GetDevelopersByTeamAsync(string teamName)
        {
            var devs = await _developerRepository.GetDevelopers();
            return devs.Where(d => d.Team == teamName).ToList();
        }

        public async Task<ITeamEntity> GetDeveloperTeamAsync(string telegramAcc)
        {
            var devs = await _developerRepository.GetDevelopers();
            var dev = devs.Where(d => d.TelegramAcc == telegramAcc).FirstOrDefault();
            var teams = await _teamRepository.GetTeams();
            return teams.Where(t => t.Name == dev.Team).FirstOrDefault();
        }

        public async Task<bool> IsDeveloperInTeamAsync(string telegramAcc, string teamName)
        {
            var devTeam = await GetDeveloperTeamAsync(telegramAcc);
            return devTeam.Name == teamName ? false : true;
        }

        public Task<bool> RemoveDeveloperAsync(string rowKey)
        {
            return _developerRepository.RemoveDeveloper(rowKey);
        }

        public Task<bool> SaveDeveloperAsync(IDeveloperEntity developer)
        {
            return _developerRepository.SaveDeveloper(developer);
        }
    }
}
