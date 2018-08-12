using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.LykkeDevelopers.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class LykkeDevelopersSettings
    {
        public DbSettings Db { get; set; }
        public string ApiClientId { get; set; }
        public string AvailableEmailsRegex { get; set; }        
        public int UserLoginTime { get; set; }
        public string DefaultPassword { get; set; }
        public string DefaultUserEmail { get; set; }
        public string DefaultUserFirstName { get; set; }
        public string DefaultUserLasttName { get; set; }
    }
}
