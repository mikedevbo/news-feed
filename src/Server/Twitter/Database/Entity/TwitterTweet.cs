namespace NewsFeed.Server.Models.Twitter.Entity
{
    public record TwitterTweet(int Id, int UserId, bool IsPersisted, bool IsRed)
    {
        public TwitterTweet() : this(default, default, default, default) { }
    }
}