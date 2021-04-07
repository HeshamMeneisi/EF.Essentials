using Serilog.Events;

namespace EF.Essentials.Config
{
    public class SlackLoggingConfig
    {
        public ChannelConfig[] Channels { get; set; }
        public LogEventLevel LogLevel { get; set; }
    }
}
