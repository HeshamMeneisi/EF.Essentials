using System.Diagnostics;
using System.Reflection;
using GenericCompany.Common.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GenericCompany.Common.StartupExtensions
{
    public static class Configuration
    {
        public static void AddBaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseConfig>(configuration.GetSection(nameof(DatabaseConfig)));
            services.Configure<LoggingConfig>(configuration.GetSection(nameof(LoggingConfig)));
            services.Configure<ServiceConfig>(configuration.GetSection(nameof(ServiceConfig)));
            services.Configure<HealthCheckConfig>(configuration.GetSection(nameof(HealthCheckConfig)));
            services.Configure<SecurityConfig>(configuration.GetSection(nameof(SecurityConfig)));
            services.Configure<CorsConfig>(configuration.GetSection(nameof(CorsConfig)));
        }
    }
}
