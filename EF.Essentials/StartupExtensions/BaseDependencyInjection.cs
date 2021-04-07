using System;
using EF.Essentials.Config;
using EF.Essentials.Encryption;
using EF.Essentials.KeyStore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EF.Essentials.StartupExtensions
{
    public static class BaseDependencyInjection
    {
        public static void InjectBaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ISignatureKeyContainer, SignatureKeyContainer>();
            services.AddSingleton<ISignatureKeyResolver, SignatureKeyResolver>();
            services.AddSingleton<IServiceTokenFactory, ServiceTokenFactory>();
            services.AddSingleton(
                SetupKeyStoreHttpClient(configuration.GetSection("ServiceConfig").Get<ServiceConfig>().KeyStore));
        }

        private static IKeyStoreClient SetupKeyStoreHttpClient(KeyStoreConfig config)
        {
            return new KeyStoreClient
            {
                BaseUri = new Uri(config.BaseUrl)
            };
        }
    }
}
