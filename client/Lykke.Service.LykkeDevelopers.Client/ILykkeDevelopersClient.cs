using Lykke.Service.LykkeDevelopers.Client.Api;

namespace Lykke.Service.LykkeDevelopers.Client
{
    /// <summary>
    /// DevelopersClient interface
    /// </summary>
    public interface ILykkeDevelopersClient
    {
        /// <summary>
        /// Api for developers management
        /// </summary>
        IDeveloperApi Developer { get; }

        /// <summary>
        /// Api for teams management
        /// </summary>
        ITeamApi Team { get; }
    }
}
