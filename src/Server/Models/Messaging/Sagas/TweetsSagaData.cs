using NServiceBus;

namespace NewsFeed.Server.Models.Messaging.Sagas
{
    public class TweetsSagaData : ContainSagaData
    {
        public string UserId { get; set; } = "";

        public List<Tweet> Tweets { get; set; } = new List<Tweet>();

        public class Tweet
        {
            public string Id { get; set; } = string.Empty;

            public string Text { get; set; } = string.Empty;

            // public string CreatedAt { get; set; }

            // ...
        }

    }
}
