namespace NewsFeed.Shared.Twitter.Model
{
    public record Group(
        int Id,
        string Name)
    {
        public Group() : this(default, string.Empty) { }
    }

    public record User(
        int Id,
        string Name,
        string TwitterUserId,
        bool IsTweetsDownloading,
        int GroupId)
    {
        public User() : this(default, string.Empty, string.Empty, default, default) { }
    }
}
