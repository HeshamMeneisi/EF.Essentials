using System;
using GenericCompany.Common.Config;
using GenericCompany.Common.Encryption;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GenericCompany.Common.Communication
{
    public static class Extensions
    {
        public static IServiceCollection AddApi<TApi>(this IServiceCollection services,
            Func<ServiceConfig, TApi> createInstance) where TApi: class
        {
            return services.AddSingleton(p =>
            {
                var config = p.GetRequiredService<IOptions<ServiceConfig>>().Value;
                return new SdkApi<TApi>(createInstance(config), p.GetRequiredService<IServiceTokenFactory>());
            });
        }
    }
}
