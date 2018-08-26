using Newtonsoft.Json;

namespace Lykke.Service.LykkeDevelopers.Client.Models
{
    /// <summary>
    /// Developer Model.
    /// </summary>
    public class DeveloperModel
    {
        /// <summary>
        /// Developer ID.
        /// </summary>
        [JsonProperty("RowKey")]
        public string RowKey { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("TelegramAcc")]
        public string TelegramAcc { get; set; }

        [JsonProperty("GithubAcc")]
        public string GithubAcc { get; set; }

        [JsonProperty("Team")]
        public string Team { get; set; }

        [JsonProperty("TeamID")]
        public string TeamID { get; set; }
    }
}
