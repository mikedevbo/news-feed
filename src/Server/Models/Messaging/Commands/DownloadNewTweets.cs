using NServiceBus;

namespace NewsFeed.Server.Models.Messaging.Commands
{
    public class DownloadNewTweets : ICommand
    {
        public string UserId { get; set; } = string.Empty;
    }
}