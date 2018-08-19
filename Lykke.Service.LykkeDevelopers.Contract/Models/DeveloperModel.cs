namespace Lykke.Service.LykkeDevelopers.Contract.Models
{
    /// <summary>
    /// Developer Model.
    /// </summary>
    public class DeveloperModel
    {
        /// <summary>
        /// Developer ID.
        /// </summary>
        public string RowKey { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string TelegramAcc { get; set; }

        public string GithubAcc { get; set; }

        public string Team { get; set; }
    }
}
