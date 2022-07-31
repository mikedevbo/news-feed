using NewsFeed.Server.Models.Messaging.Sagas;
using NServiceBus;

namespace NewsFeed.Server.Models.Messaging.Messages
{
    public class GetDownloadedTweetsRequest : IMessage
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class GetDownloadedTweetsResponse : IMessage
    {
        public TweetsSagaData? Data { get; set; }
    }
}
