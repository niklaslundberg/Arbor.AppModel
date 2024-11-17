using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;

namespace Arbor.AppModel.Http
{
    public static class HttpClientRegistrationExtensions
    {
        public static IServiceCollection AddHttpClientsWithConfiguration(this IServiceCollection services,
            [NotNull] HttpLoggingConfiguration httpLoggingConfiguration)
        {
            ArgumentNullException.ThrowIfNull(httpLoggingConfiguration);

            services.AddHttpClient();

            if (!httpLoggingConfiguration.Enabled)
            {
                services.Replace(ServiceDescriptor.Singleton<IHttpMessageHandlerBuilderFilter, CustomLoggingFilter>());
            }

            return services;
        }
    }
}