using Lykke.HttpClientGenerator;

namespace Lykke.Service.LykkeDevelopers.Client
{
    /// <summary>
    /// LykkeDevelopers API aggregating interface.
    /// </summary>
    public class LykkeDevelopersClient : ILykkeDevelopersClient
    {
        // Note: Add similar Api properties for each new service controller

        /// <summary>Inerface to LykkeDevelopers Api.</summary>
        public ILykkeDevelopersApi Api { get; private set; }

        /// <summary>C-tor</summary>
        public LykkeDevelopersClient(IHttpClientGenerator httpClientGenerator)
        {
            Api = httpClientGenerator.Generate<ILykkeDevelopersApi>();
        }
    }
}
