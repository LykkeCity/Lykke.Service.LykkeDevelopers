using AzureStorage;
using Lykke.Service.LykkeDevelopers.Core.Team;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.AzureRepositories.Team
{
    public class TeamRepository : ITeamRepository
    {
        private readonly INoSQLTableStorage<TeamEntity> _tableStorage;

        public TeamRepository(INoSQLTableStorage<TeamEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<ITeamEntity> GetTeamAsync(string rowKey)
        {
            var pk = TeamEntity.GeneratePartitionKey();
            return await _tableStorage.GetDataAsync(pk, TeamEntity.GenerateRowKey(rowKey));
        }

        public async Task<List<ITeamEntity>> GetTeams()
        {
            var pk = TeamEntity.GeneratePartitionKey();
            return (await _tableStorage.GetDataAsync(pk)).Cast<ITeamEntity>().ToList();
        }

        public async Task<bool> RemoveTeam(string rowKey)
        {
            try
            {
                await _tableStorage.DeleteAsync(TeamEntity.GeneratePartitionKey(), TeamEntity.GenerateRowKey(rowKey));
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> SaveTeam(ITeamEntity team)
        {
            try
            {
                var teamToSave = (TeamEntity)team;
                if (String.IsNullOrWhiteSpace(teamToSave.RowKey))
                {
                    teamToSave.RowKey = Guid.NewGuid().ToString();
                }

                if (String.IsNullOrWhiteSpace(teamToSave.PartitionKey))
                {
                    teamToSave.PartitionKey = TeamEntity.GeneratePartitionKey();
                }
                await _tableStorage.InsertOrMergeAsync(teamToSave);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
