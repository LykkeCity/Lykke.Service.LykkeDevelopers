using Autofac;
using Common.Log;
using Lykke.Service.LykkeDevelopers.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.LykkeDevelopers
{
    public class AutofacModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;
        private readonly ILog _log;

        public AutofacModule(IReloadingManager<AppSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();
        }
    }
}
