namespace EF.Essentials.Config
{
    public class LoggingConfig
    {
        public ESLoggingConfig ElasticSearch { get; set; }
        public SlackLoggingConfig Slack { get; set; }
    }
}
