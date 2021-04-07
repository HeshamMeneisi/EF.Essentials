using EF.Essentials.Config;
using EF.Essentials.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EF.Essentials.StartupExtensions
{
    public static class Workers
    {
        public static void RegisterBaseWorkers(this IServiceCollection services, IConfiguration configuration)
        {
            var securityConfig = configuration.GetSection(nameof(SecurityConfig)).Get<SecurityConfig>();
            if(securityConfig.CanIssueTokens)
                services.AddHostedService<SignatureKeyGenerationWorker>();
        }
    }
}
