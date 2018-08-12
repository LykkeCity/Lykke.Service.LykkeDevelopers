using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.LykkeDevelopers.Client 
{
    /// <summary>
    /// LykkeDevelopers client settings.
    /// </summary>
    public class LykkeDevelopersServiceClientSettings 
    {
        /// <summary>Service url.</summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl {get; set;}
    }
}
