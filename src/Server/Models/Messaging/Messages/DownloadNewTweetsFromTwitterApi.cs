using NServiceBus;

namespace NewsFeed.Server.Models.Messaging.Messages
{
    public class DownloadNewTweetsFromTwitterApiRequest : IMessage
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class DownloadNewTweetsFromTwitterApiResponse : IMessage
    {
        public List<Tweet> Tweets { get; set; } = new List<Tweet>();

        public class Tweet
        {
            public string Id { get; set; } = string.Empty;

            public string Text { get; set; } = string.Empty;

            public string AuthorId { get; set; } = string.Empty;
        }
    }
}
