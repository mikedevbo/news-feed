using System.ComponentModel;

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

    public record Tweet(
        int Id,
        int UserId,
        bool IsPersisted,
        string TweetId,
        string Text,
        DateTime? CreatedAt)
    {
        public Tweet() : this(default, default, default, string.Empty, string.Empty, default) { }

        public bool IsRead { get; set; }
    }
}
