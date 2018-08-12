﻿using AzureStorage;
using Lykke.Service.LykkeDevelopers.Core.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.AzureRepositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly INoSQLTableStorage<UserEntity> _tableStorage;

        public UserRepository(INoSQLTableStorage<UserEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IUserEntity> GetUserByUserEmail(string userEmail)
        {
            var pk = UserEntity.GeneratePartitionKey();
            return await _tableStorage.GetDataAsync(pk, UserEntity.GenerateRowKey(userEmail));
        }

        public async Task<IUserEntity> GetUserByUserEmail(string userEmail, string passwordHash)
        {
            var pk = UserEntity.GeneratePartitionKey();
            var result = await _tableStorage.GetDataAsync(pk, UserEntity.GenerateRowKey(userEmail));
            if (result == null)
            {
                return null;
            }
            return result.PasswordHash.Equals(passwordHash) ? result : null;
        }

        public async Task<bool> SaveUser(IUserEntity user)
        {
            try
            {
                var te = (UserEntity)user;
                te.RowKey = UserEntity.GenerateRowKey(te.RowKey);
                if (te.PartitionKey == null)
                {
                    te.PartitionKey = UserEntity.GeneratePartitionKey();
                }
                await _tableStorage.InsertOrMergeAsync(te);
            }


            catch
            {
                return false;
            }

            return true;
        }

        public async Task<List<IUserEntity>> GetUsers()
        {
            var pk = UserEntity.GeneratePartitionKey();
            return (await _tableStorage.GetDataAsync(pk)).Cast<IUserEntity>().ToList();
        }

        public async Task<bool> RemoveUser(string userEmail)
        {
            try
            {
                await _tableStorage.DeleteAsync(UserEntity.GeneratePartitionKey(), UserEntity.GenerateRowKey(userEmail));
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
