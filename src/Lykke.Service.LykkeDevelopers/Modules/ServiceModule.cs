using Autofac;
using Lykke.Service.LykkeDevelopers.Core.Services;
using Lykke.Service.LykkeDevelopers.Services;
using Lykke.Service.LykkeDevelopers.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.LykkeDevelopers.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;

        public ServiceModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<DevelopersService>()
               .As<IDevelopersService>()
               .SingleInstance();

            builder.RegisterType<TeamsService>()
               .As<ITeamsService>()
               .SingleInstance();

            builder.RegisterInstance(_appSettings.CurrentValue)
                    .AsSelf()
                    .SingleInstance();
        }
    }
}
