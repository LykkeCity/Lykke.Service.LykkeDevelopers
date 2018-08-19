using Lykke.Service.LykkeDevelopers.Client.Api;
using Lykke.Service.LykkeDevelopers.Contract.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
