using Dapper.Contrib.Extensions;

namespace NewsFeed.Server.Models.Twitter.Tables
{
    public class TwitterTweetsApi
    {
        [ExplicitKey]
        public int Id { get; set; }

        public string TweetId { get; set; } = string.Empty;

        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}