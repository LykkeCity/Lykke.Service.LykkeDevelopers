using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.LykkeDevelopers.Client
{
    /// <summary>
    /// Settings for <see cref="ILykkeDevelopersClient"/>.
    /// </summary>
    public class LykkeDevelopersServiceClientSettings
    {
        /// <summary>Service url.</summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl { get; set; }
    }
}
