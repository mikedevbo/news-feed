using Dapper.Contrib.Extensions;

namespace NewsFeed.Server.Models.Twitter.Entity
{
    [Table("TwitterUsersApi")]
    public record TwitterTweetsApi([property: ExplicitKey] int Id, string TweetId, string Text, DateTime CreatedAt)
    {
        public TwitterTweetsApi() : this(default, string.Empty, string.Empty, default) { }
    }
}