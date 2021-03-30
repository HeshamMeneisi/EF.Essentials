using Serilog.Events;

namespace GenericCompany.Common.Config
{
    public class SlackLoggingConfig
    {
        public ChannelConfig[] Channels { get; set; }
        public LogEventLevel LogLevel { get; set; }
    }
}
