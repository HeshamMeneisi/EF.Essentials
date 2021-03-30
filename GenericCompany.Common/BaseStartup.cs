using System.Text.Json.Serialization;
using App.Metrics;
using GenericCompany.Common.StartupExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace GenericCompany.Common
{
    public class BaseStartup
    {
        public BaseStartup(IConfiguration configuration)
        {
            Configuration = configuration;

            this.ConfigureSerilog(configuration);
        }

        public IConfiguration Configuration { get; }
        public IHealthChecksBuilder HealthCheckBuilder { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsPolicies(Configuration);
            services.AddBaseConfiguration(Configuration);
            var metricsOptions = Configuration.GetSection("MetricsOptions").Get<MetricsOptions>();
            if (metricsOptions.Enabled)
            {
                services.AddMetrics();
            }
            services.ConfigureIdentityServices();
            services.AddMvc().AddJsonOptions(options =>
            {
                var converter = new JsonStringEnumConverter();
                options.JsonSerializerOptions.Converters.Add(converter);
            });
            services.ConfigureSwagger();
            services.RegisterBaseWorkers();
            HealthCheckBuilder = services.ConfigureHealthChecks(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IHostApplicationLifetime appLifetime)
        {
            app.ConfigureGlobalExceptionHandler();
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseCorsPolicy(env);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.AddSwaggerWithUi();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.ConfigureHealthCheckEndpoint();
            });

            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
        }
    }
}
