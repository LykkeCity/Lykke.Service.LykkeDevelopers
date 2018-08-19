using Autofac;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.LykkeDevelopers.AzureRepositories.Developer;
using Lykke.Service.LykkeDevelopers.AzureRepositories.User;
using Lykke.Service.LykkeDevelopers.Core.Developer;
using Lykke.Service.LykkeDevelopers.Core.User;
using Lykke.Service.LykkeDevelopers.Settings;
using Lykke.SettingsReader;


namespace Lykke.Service.LykkeDevelopers.Modules
{
    public class DbModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;
        private readonly ILog _log;

        public DbModule(IReloadingManager<AppSettings> appSettings, ILog log)
        {
            _appSettings = appSettings;
            _log = log;

        }

        protected override void Load(ContainerBuilder builder)
        {
            var connectionString = _appSettings.ConnectionString(x => x.LykkeDevelopersService.Db.ConnectionString);
            var userConnectionString = _appSettings.ConnectionString(x => x.LykkeDevelopersService.Db.UserConnectionString);

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.Register(c =>
                new UserRepository(AzureTableStorage<UserEntity>.Create(userConnectionString,
                        "User",
                        c.Resolve<ILogFactory>())))
                        .As<IUserRepository>()
                        .SingleInstance();

            builder.Register(c =>
               new DeveloperRepository(AzureTableStorage<DeveloperEntity>.Create(userConnectionString,
                       "Developer",
                       c.Resolve<ILogFactory>())))
                       .As<IDeveloperRepository>()
                       .SingleInstance();

        }
    }
}
