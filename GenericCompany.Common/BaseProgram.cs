using System;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Extensions.Configuration;
using App.Metrics.Formatters.InfluxDB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace GenericCompany.Common
{
    public class BaseProgram
    {
        public static void ConfigureMetrics(IWebHostBuilder webBuilder)
        {
            webBuilder.ConfigureMetricsWithDefaults((context, builder) =>
            {
                builder.Configuration.ReadFrom(context.Configuration);
                builder.Report.ToInfluxDb(options =>
                {
                    options.FlushInterval = TimeSpan.FromSeconds(5);
                    context.Configuration.GetSection("MetricsOptions").Bind(options);
                    options.MetricsOutputFormatter = new MetricsInfluxDbLineProtocolOutputFormatter();
                });
            });
            webBuilder.UseMetrics();
        }
    }
}
