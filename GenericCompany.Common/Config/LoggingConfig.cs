namespace GenericCompany.Common.Config
{
    public class LoggingConfig
    {
        public ESLoggingConfig ElasticSearch { get; set; }
        public SlackLoggingConfig Slack { get; set; }
    }
}
