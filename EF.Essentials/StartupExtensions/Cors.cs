using EF.Essentials.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EF.Essentials.StartupExtensions
{
    public static class Cors
    {
        private static CorsConfig _corsConfig;

        public static void AddCorsPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            _corsConfig = configuration.GetSection(nameof(CorsConfig)).Get<CorsConfig>();

            services.AddCors(x =>
            {
                x.AddPolicy("development", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .WithHeaders("authorization", "content-type", "x-ms-command-name")
                        .AllowAnyMethod();
                });
                x.AddPolicy("production", builder =>
                {
                    if (_corsConfig.AllowedOrigins.Length > 0)
                        builder.WithOrigins(_corsConfig.AllowedOrigins);
                    else
                        builder.AllowAnyOrigin();

                    builder
                        .WithHeaders(_corsConfig.AllowedHeaders)
                        .AllowAnyMethod();
                });
            });
        }

        public static void UseCorsPolicy(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(env.IsProduction() && _corsConfig.Enabled ? "production" : "development");
        }
    }
}
