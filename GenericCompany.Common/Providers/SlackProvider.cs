using Slack.Webhooks;

namespace GenericCompany.Common.Providers
{
    public class SlackProvider
    {
        private Slack.Webhooks.SlackClient _slackClient;

        public SlackProvider(string webhook)
        {
            _slackClient = new Slack.Webhooks.SlackClient(webhook);
        }

        public Slack.Webhooks.SlackClient Client
        {
            get { return _slackClient; }
        }
    }
    public static class ReportsMessageFactory
    {
        public static SlackMessage Create(string message, string? emoji = null)
        {
            return new SlackMessage
            {
                Channel = "#reports",
                Text = message,
                IconEmoji = emoji ?? Emoji.New,
                Username = "Report Bot"
            };
        }
    }
}
