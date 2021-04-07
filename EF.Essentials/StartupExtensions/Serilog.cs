using System;
using System.Linq;
using System.Reflection;
using Amazon;
using Amazon.Runtime;
using EF.Essentials.Config;
using Elasticsearch.Net;
using Elasticsearch.Net.Aws;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.Slack;

namespace EF.Essentials.StartupExtensions
{
    public static class Serilog
    {
        private static string ServiceName = Assembly.GetEntryAssembly().GetName().Name;

        public static void ConfigureSerilog(this BaseStartup _, IConfiguration configuration)
        {
            var index = ServiceName.Replace(".", "-");
            var loggingConfig = configuration.GetSection(nameof(LoggingConfig)).Get<LoggingConfig>();

            var esConfig = loggingConfig.ElasticSearch;
            var slackConfig = loggingConfig.Slack;

            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Warning();

            if (!string.IsNullOrEmpty(esConfig.ElasticUrl))
                logger = logger.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(esConfig.ElasticUrl))
                {
                    ModifyConnectionSettings = conn =>
                    {
                        var awsCredentials =
                            new BasicAWSCredentials(esConfig.AwsAccessKeyId, esConfig.AwsSecretKey);
                        var httpConnection = new AwsHttpConnection(awsCredentials,
                            RegionEndpoint.GetBySystemName(esConfig.AwsRegion));
                        var pool = new SingleNodeConnectionPool(new Uri(esConfig.ElasticUrl));

                        return new ConnectionConfiguration(pool, httpConnection);
                    },
                    IndexFormat = $"{index}-{{0:yyyy.MM}}",
                    AutoRegisterTemplate = true
                });

            if (slackConfig.Channels.Length > 0 && !string.IsNullOrEmpty(slackConfig.Channels[0].WebhookUrl))
                logger = logger.WriteTo.Slack(
                    (SlackChannelCollection) slackConfig.Channels.Select(c => new SlackChannel(c.WebhookUrl)), null,
                    slackConfig.LogLevel);

            Log.Logger = logger.CreateLogger();
        }
    }
}
