﻿using Autofac;
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
            // Do not register entire settings in container, pass necessary settings to services which requires them
            builder.RegisterInstance(_appSettings.CurrentValue)
                    .AsSelf()
                    .SingleInstance();
        }
    }
}
