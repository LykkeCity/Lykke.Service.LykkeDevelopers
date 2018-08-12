using AzureStorage;
using Lykke.Service.LykkeDevelopers.Core.Developer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.AzureRepositories.Developer
{
    public class DeveloperRepository : IDeveloperRepository
    {
        private readonly INoSQLTableStorage<DeveloperEntity> _tableStorage;

        public DeveloperRepository(INoSQLTableStorage<DeveloperEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IDeveloperEntity> GetDevAsync(string rowKey)
        {
            var pk = DeveloperEntity.GeneratePartitionKey();
            return await _tableStorage.GetDataAsync(pk, DeveloperEntity.GenerateRowKey(rowKey));
        }

        public Task<IDeveloperEntity> GetDevByTelegramAcc(string telegramAcc)
        {
            throw new NotImplementedException();
        }

        public async Task<List<IDeveloperEntity>> GetDevelopers()
        {
            var pk = DeveloperEntity.GeneratePartitionKey();
            return (await _tableStorage.GetDataAsync(pk)).Cast<IDeveloperEntity>().ToList();
        }

        public async Task<bool> RemoveDeveloper(string rowKey)
        {
            try
            {
                await _tableStorage.DeleteAsync(DeveloperEntity.GeneratePartitionKey(), DeveloperEntity.GenerateRowKey(rowKey));
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> SaveDeveloper(IDeveloperEntity developer)
        {
            try
            {
                var dev = (DeveloperEntity)developer;
                if( dev.RowKey == null)
                {
                    dev.RowKey = Guid.NewGuid().ToString();
                }
                
                if (dev.PartitionKey == null)
                {
                    dev.PartitionKey = DeveloperEntity.GeneratePartitionKey();
                }
                await _tableStorage.InsertOrMergeAsync(dev);
            }


            catch
            {
                return false;
            }

            return true;
        }
    }
}
