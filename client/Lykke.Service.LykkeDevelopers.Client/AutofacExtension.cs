using Autofac;
using JetBrains.Annotations;
using Lykke.HttpClientGenerator;
using Lykke.HttpClientGenerator.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.LykkeDevelopers.Client
{
    /// <summary>
    /// Autofac extension to register client dialogs client
    /// </summary>
    [PublicAPI]
    public static class AutofacExtension
    {
        /// <summary>
        /// Registers <see cref="ILykkeDevelopersClient"/> in Autofac container using <see cref="LykkeDevelopersServiceClientSettings"/>.
        /// </summary>
        /// <param name="builder">Autofac container builder.</param>
        /// <param name="settings">LykkeDevelopers client settings.</param>
        /// <param name="builderConfigure">Optional <see cref="HttpClientGeneratorBuilder"/> configure handler.</param>
        public static void RegisterLykkeDevelopersClient(
            [NotNull] this ContainerBuilder builder,
            [NotNull] LykkeDevelopersServiceClientSettings settings,
            [CanBeNull] Func<HttpClientGeneratorBuilder, HttpClientGeneratorBuilder> builderConfigure)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            if (string.IsNullOrWhiteSpace(settings.ServiceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(LykkeDevelopersServiceClientSettings.ServiceUrl));
            var clientBuilder = HttpClientGenerator.HttpClientGenerator.BuildForUrl(settings.ServiceUrl)
               .WithAdditionalCallsWrapper(new ExceptionHandlerCallsWrapper());
            clientBuilder = builderConfigure?.Invoke(clientBuilder) ?? clientBuilder.WithoutRetries();
            builder.RegisterInstance(new LykkeDevelopersClient(clientBuilder.Create()))
               .As<ILykkeDevelopersClient>()
               .SingleInstance();
        }
    }
}
