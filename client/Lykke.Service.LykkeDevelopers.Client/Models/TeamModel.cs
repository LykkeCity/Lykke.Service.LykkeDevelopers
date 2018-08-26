using Newtonsoft.Json;

namespace Lykke.Service.LykkeDevelopers.Client.Models
{
    /// <summary>
    /// Team Model.
    /// </summary>
    public class TeamModel
    {
        /// <summary>
        /// Team ID.
        /// </summary>
        [JsonProperty("RowKey")]
        public string RowKey { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}
