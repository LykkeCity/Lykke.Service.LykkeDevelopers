using Lykke.Service.LykkeDevelopers.Core.Domain.Team;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace Lykke.Service.LykkeDevelopers.AzureRepositories.Team
{
    public class TeamEntity : TableEntity, ITeamEntity
    {
        public static string GeneratePartitionKey() => "Team";

        public static string GenerateRowKey(string rowKey) => rowKey;

        public string Name { get; set; }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            if (properties.TryGetValue("Name", out var name))
            {
                Name = name.StringValue;
            }
        }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var dict = new Dictionary<string, EntityProperty>
            {
                {"Name", new EntityProperty(Name)},
            };

            return dict;
        }
    }
}
