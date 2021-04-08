using System.Linq;
using System.Threading.Tasks;
using EF.Essentials.Config;
using EF.Essentials.HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EF.Essentials.StartupExtensions
{
    public static class HealthChecks
    {
        private static HealthCheckConfig _config;

        public static IHealthChecksBuilder ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            _config = configuration.GetSection("HealthCheckConfig").Get<HealthCheckConfig>();

            MemoryCheck.Threshold = _config.MemoryCheckConfig.MemoryUsageThresholdMb;

            var context = services
                .AddHealthChecks();

            if (_config.MemoryCheckConfig.Enabled)
            {
                context = context.AddCheck<MemoryCheck>(nameof(MemoryCheck));
            }

            return context;
        }

        public static void ConfigureHealthCheckEndpoint(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks(_config.Endpoint, new HealthCheckOptions
            {
                ResponseWriter = WriteResponse,
                AllowCachingResponses = false
            });
        }

        private static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));
            return httpContext.Response.WriteAsync(
                json.ToString(Formatting.Indented));
        }
    }
}
