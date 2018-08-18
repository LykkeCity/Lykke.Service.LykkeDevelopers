using Autofac;
using AzureStorage.Tables;
using Lykke.Common.Log;
using Lykke.Service.LykkeDevelopers.AzureRepositories.Developer;
using Lykke.Service.LykkeDevelopers.AzureRepositories.Team;
using Lykke.Service.LykkeDevelopers.AzureRepositories.User;
using Lykke.Service.LykkeDevelopers.Core.Developer;
using Lykke.Service.LykkeDevelopers.Core.Team;
using Lykke.Service.LykkeDevelopers.Core.User;
using Lykke.Service.LykkeDevelopers.Settings;
using Lykke.SettingsReader;


namespace Lykke.Service.LykkeDevelopers.Modules
{
    public class DbModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;

        public DbModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;

        }

        protected override void Load(ContainerBuilder builder)
        {
            var connectionString = _appSettings.ConnectionString(x => x.LykkeDevelopersService.Db.ConnectionString);
            var userConnectionString = _appSettings.ConnectionString(x => x.LykkeDevelopersService.Db.UserConnectionString);

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
            builder.Register(c =>
               new TeamRepository(AzureTableStorage<TeamEntity>.Create(userConnectionString,
                       "Team",
                       c.Resolve<ILogFactory>())))
                       .As<ITeamRepository>()
                       .SingleInstance();

        }
    }
}
