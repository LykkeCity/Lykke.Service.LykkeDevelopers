using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.LykkeDevelopers.Contract.Models
{
    public class DeveloperModel
    {
        public string RowKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string TelegramAcc { get; set; }
        public string GithubAcc { get; set; }
        public string Team { get; set; }
    }
}
