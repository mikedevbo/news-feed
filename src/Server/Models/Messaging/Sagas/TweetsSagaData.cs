using NServiceBus;

namespace NewsFeed.Server.Models.Messaging.Sagas
{
    public class TweetsSagaData : ContainSagaData
    {
        public string UserId { get; set; } = "";

        public List<Tweet> Tweets { get; set; } = new List<Tweet>();

        public class Tweet
        {
            public string Text { get; set; } = "";

            // public string CreatedAt { get; set; }

            // ...
        }

    }
}
