using JetBrains.Annotations;
using Lykke.Sdk.Settings;

namespace Lykke.Service.LykkeDevelopers.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public LykkeDevelopersSettings LykkeDevelopersService { get; set; }
    }
}
