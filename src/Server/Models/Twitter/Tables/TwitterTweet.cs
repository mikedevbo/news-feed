using Dapper.Contrib.Extensions;

namespace NewsFeed.Server.Models.Twitter.Tables
{
    public class TwitterTweet
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public bool IsPersisted { get; set; }
    }
}
