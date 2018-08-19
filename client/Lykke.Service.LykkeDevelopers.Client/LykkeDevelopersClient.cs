using Lykke.HttpClientGenerator;
using Lykke.Service.LykkeDevelopers.Client.Api;

namespace Lykke.Service.LykkeDevelopers.Client
{
    /// <summary>
    /// LykkeDevelopers API aggregating interface.
    /// </summary>
    public class LykkeDevelopersClient : ILykkeDevelopersClient
    {
        private HttpClientGenerator.HttpClientGenerator httpClientGenerator;

        public IDeveloperApi Developer { get; set; }

        public ITeamApi Team { get; set; }

        public LykkeDevelopersClient(LykkeDevelopersServiceClientSettings settings)
        {
            Developer = httpClientGenerator.Generate<IDeveloperApi>();
            Team = httpClientGenerator.Generate<ITeamApi>();
        }

        public LykkeDevelopersClient(HttpClientGenerator.HttpClientGenerator httpClientGenerator)
        {
            this.httpClientGenerator = httpClientGenerator;
        }
    }
}
