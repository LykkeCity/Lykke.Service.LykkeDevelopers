using Lykke.Service.LykkeDevelopers.Core.Domain.Developer;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace Lykke.Service.LykkeDevelopers.AzureRepositories.Developer
{
    public class DeveloperEntity : TableEntity, IDeveloperEntity
    {
        public static string GeneratePartitionKey() => "Dev";

        public static string GenerateRowKey(string rowKey) => rowKey;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string TelegramAcc { get; set; }
        public string GithubAcc { get; set; }
        public string Team { get; set; }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            if (properties.TryGetValue("Email", out var email))
            {
                Email = email.StringValue;
            }

            if (properties.TryGetValue("TelegramAcc", out var telegramAcc))
            {
                TelegramAcc = telegramAcc.StringValue;
            }

            if (properties.TryGetValue("FirstName", out var firstName))
            {
                FirstName = firstName.StringValue;
            }

            if (properties.TryGetValue("LastName", out var lastName))
            {
                LastName = lastName.StringValue;
            }

            if (properties.TryGetValue("GithubAcc", out var githubAcc))
            {
                GithubAcc = githubAcc.StringValue;
            }

            if (properties.TryGetValue("Team", out var team))
            {
                Team = team.StringValue;
            }

        }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var dict = new Dictionary<string, EntityProperty>
            {
                {"Email", new EntityProperty(Email)},
                {"TelegramAcc", new EntityProperty(TelegramAcc)},
                {"FirstName", new EntityProperty(FirstName)},
                {"LastName", new EntityProperty(LastName)},
                {"GithubAcc", new EntityProperty(GithubAcc)},
                {"Team", new EntityProperty(Team)}
            };

            return dict;
        }
    }
}
