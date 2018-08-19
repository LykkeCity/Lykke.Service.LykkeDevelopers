
namespace Lykke.Service.LykkeDevelopers.Client
{
    /// <summary>
    /// Settings for <see cref="ILykkeDevelopersClient"/>.
    /// </summary>
    public class LykkeDevelopersServiceClientSettings
    {
        /// <summary>
        /// Initializes a new instance of <see cref="LykkeDevelopersServiceClientSettings"/>.
        /// </summary>
        public LykkeDevelopersServiceClientSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="LykkeDevelopersServiceClientSettings"/> with service url.
        /// </summary>
        public LykkeDevelopersServiceClientSettings(string serviceUrl)
        {
            ServiceUrl = serviceUrl;
        }

        /// <summary>
        /// The service url.
        /// </summary>
        public string ServiceUrl { get; set; }
    }
}
