using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.LykkeDevelopers.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
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

namespace Lykke.Service.LykkeDevelopers
{
    [UsedImplicitly]
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public ILogFactory LogFactory { get; private set; }


        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        private readonly LykkeSwaggerOptions _swaggerOptions = new LykkeSwaggerOptions
        {
            ApiTitle = "LykkeDevelopers API",
            ApiVersion = "v1"
        };

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

            var builder = new ContainerBuilder();

            builder.RegisterModule(new DbModule(_settings));
            builder.Populate(services);

            return services.BuildServiceProvider<AppSettings>(options =>
            {
                options.SwaggerOptions = _swaggerOptions;

                options.Logs = logs =>
                {
                    logs.AzureTableName = "LykkeDevelopersLog";
                    logs.AzureTableConnectionStringResolver = settings => settings.LykkeDevelopersService.Db.LogsConnString;
                };
            });
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseLykkeConfiguration();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Developers}/{id?}");
            });
        }
    }
}
