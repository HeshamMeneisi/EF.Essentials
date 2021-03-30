using System;
using GenericCompany.Common.Config;
using GenericCompany.Common.Encryption;
using GenericCompany.Common.KeyStore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GenericCompany.Common.StartupExtensions
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
