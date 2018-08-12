using JetBrains.Annotations;

namespace Lykke.Service.LykkeDevelopers.Client
{
    /// <summary>
    /// LykkeDevelopers client interface.
    /// </summary>
    [PublicAPI]
    public interface ILykkeDevelopersClient
    {
        /// <summary>Application Api interface</summary>
        ILykkeDevelopersApi Api { get; }
    }
}
