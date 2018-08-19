using JetBrains.Annotations;
using Lykke.Logs.Loggers.LykkeSlack;
using Lykke.Sdk;
using Lykke.Sdk.Health;
using Lykke.Sdk.Middleware;
using Lykke.Service.LykkeDevelopers.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Hosting;
using Lykke.SettingsReader.ReloadingManager;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Lykke.Service.LykkeDevelopers.Modules;
using Microsoft.Extensions.Configuration;
using Lykke.Common.Log;
using Swashbuckle.AspNetCore.Swagger;
using Lykke.Common.ApiLibrary.Swagger;
using Lykke.Service.LykkeDevelopers.Swagger;
using Lykke.Common.ApiLibrary.Middleware;
using Common.Log;
using Lykke.SettingsReader;
using Lykke.SlackNotification.AzureQueue;
using Lykke.Logs;
using AzureStorage.Tables;

namespace Lykke.Service.LykkeDevelopers
{
    [UsedImplicitly]
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }
        public ILogFactory LogFactory { get; private set; }
        public ILog Log { get; private set; }


        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        //private readonly LykkeSwaggerOptions _swaggerOptions = new LykkeSwaggerOptions
        //{
        //    ApiTitle = "LykkeDevelopers API",
        //    ApiVersion = "v1"
        //};

        [UsedImplicitly]
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var appSettings = Configuration.Get<AppSettings>();

            var _settings = ConstantReloadingManager.From(appSettings);

            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            })
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>

                    {
                        o.LoginPath = new PathString("/Account/SignIn");
                        o.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
            });

            services.AddDataProtection()
                .SetApplicationName("LykkeDevelopers")
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo(@"/var/dpkeys/"));

            services.AddMvc();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new Info
                    {
                        Version = "v1",
                        Title = "Lykke.Service.LykkeDevelopers API"
                    });
                options.DescribeAllEnumsAsStrings();
                options.EnableXmsEnumExtension();
                options.EnableXmlDocumentation();

                options.OperationFilter<FileUploadOperation>();
            });

            var builder = new ContainerBuilder();

            Log = CreateLogWithSlack(services, _settings);

            builder.RegisterModule(new DbModule(_settings, Log));
            builder.Populate(services);
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
            #region commented
            //return services.BuildServiceProvider<AppSettings>(options =>
            //{
            //    //options.SwaggerOptions = _swaggerOptions;
            //    options.Logs = logs =>
            //    {
            //        logs.AzureTableName = "LykkeDevelopersLog";
            //        logs.AzureTableConnectionStringResolver = settings => settings.LykkeDevelopersService.Db.LogsConnString;

            //        // TODO: You could add extended logging configuration here:
            //        /* 
            //        logs.Extended = extendedLogs =>
            //        {
            //            // For example, you could add additional slack channel like this:
            //            extendedLogs.AddAdditionalSlackChannel("LykkeDevelopers", channelOptions =>
            //            {
            //                channelOptions.MinLogLevel = LogLevel.Information;
            //            });
            //        };
            //        */
            //    };
            //});
            #endregion
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseLykkeMiddleware("Lykke.Service.LykkeDevelopers", ex => new
            {
                Message = "Technical problem"
            });

            app.UseHttpsRedirection();
            //app.UseSwagger(c =>
            //{
            //    c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
            //});
            //app.UseSwaggerUI(x =>
            //{
            //    x.RoutePrefix = "swagger/ui";
            //    x.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            //});
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Developers}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.RoutePrefix = "swagger/ui";
                x.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
            });
        }

        private static ILog CreateLogWithSlack(IServiceCollection services, IReloadingManager<AppSettings> settings)
        {
            var consoleLogger = new LogToConsole();
            var aggregateLogger = new AggregateLogger();

            aggregateLogger.AddLog(consoleLogger);

            var vl = settings.CurrentValue;
            Console.WriteLine(vl);

            // Creating slack notification service, which logs own azure queue processing messages to aggregate log
            var slackService =
                services.UseSlackNotificationsSenderViaAzureQueue(settings.CurrentValue.SlackNotifications.AzureQueue,
                    aggregateLogger);

            var dbLogConnectionStringManager = settings.Nested(x => x.LykkeDevelopersService.Db.LogsConnString);
            var dbLogConnectionString = dbLogConnectionStringManager.CurrentValue;

            // Creating azure storage logger, which logs own messages to concole log
            if (!string.IsNullOrEmpty(dbLogConnectionString) &&
                !(dbLogConnectionString.StartsWith("${") && dbLogConnectionString.EndsWith("}")))
            {
                var persistenceManager = new LykkeLogToAzureStoragePersistenceManager(
                    AzureTableStorage<LogEntity>.Create(dbLogConnectionStringManager, "PayInvoiceLog",
                        consoleLogger),
                    consoleLogger);

                var slackNotificationsManager =
                    new LykkeLogToAzureSlackNotificationsManager(slackService, consoleLogger);

                var azureStorageLogger = new LykkeLogToAzureStorage(
                    persistenceManager,
                    slackNotificationsManager,
                    consoleLogger);

                azureStorageLogger.Start();

                aggregateLogger.AddLog(azureStorageLogger);
            }

            return aggregateLogger;
        }
    }
}
