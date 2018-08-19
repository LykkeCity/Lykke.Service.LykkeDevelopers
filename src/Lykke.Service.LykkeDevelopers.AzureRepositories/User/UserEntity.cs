using Lykke.Service.LykkeDevelopers.Core.Domain.User;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace Lykke.Service.LykkeDevelopers.AzureRepositories.User
{
    public class UserEntity : TableEntity, IUserEntity
    {
        public static string GeneratePartitionKey() => "U";

        public static string GenerateRowKey(string userEmail) => userEmail.ToLowerInvariant();

        public string Salt { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Active { get; set; }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            if (properties.TryGetValue("Salt", out var salt))
            {
                Salt = salt.StringValue;
            }

            if (properties.TryGetValue("PasswordHash", out var passwordHash))
            {
                PasswordHash = passwordHash.StringValue;
            }

            if (properties.TryGetValue("FirstName", out var firstName))
            {
                FirstName = firstName.StringValue;
            }

            if (properties.TryGetValue("LastName", out var lastName))
            {
                LastName = lastName.StringValue;
            }

            if (properties.TryGetValue("Active", out var active))
            {
                Active = active.BooleanValue;
            }

        }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var dict = new Dictionary<string, EntityProperty>
            {
                {"Salt", new EntityProperty(Salt)},
                {"PasswordHash", new EntityProperty(PasswordHash)},
                {"FirstName", new EntityProperty(FirstName)},
                {"LastName", new EntityProperty(LastName)},
                {"Active", new EntityProperty(Active)},
            };

            return dict;
        }
    }
}
