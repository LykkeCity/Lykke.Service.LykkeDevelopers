using Lykke.Service.LykkeDevelopers.Core.Domain.Developer;
using Lykke.Service.LykkeDevelopers.Core.Domain.Team;
using Lykke.Service.LykkeDevelopers.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public Task<IDeveloperEntity> GetDevAsync(string rowKey)
        {
            return _developerRepository.GetDevAsync(rowKey);
        }

        public Task<List<IDeveloperEntity>> GetDevelopers()
        {
            return _developerRepository.GetDevelopers();
        }

        public Task<bool> RemoveDeveloper(string rowKey)
        {
            return _developerRepository.RemoveDeveloper(rowKey);
        }

        public Task<bool> SaveDeveloper(IDeveloperEntity developer)
        {
            return _developerRepository.SaveDeveloper(developer);
        }
    }
}
