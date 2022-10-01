using Dapper.Contrib.Extensions;

namespace NewsFeed.Server.Models.Twitter.Tables
{
    [Table("TwitterTweets")]
    public class TwitterTweets
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public bool IsPersisted { get; set; }
    }
}
