namespace NewsFeed.Shared.Twitter.Dto
{
    public record Group(
        int Id,
        string Name
    )
    {
        public Group() : this(default, string.Empty) { }
    }

    public record User(
        int Id,
        string Name,
        string TwitterUserId,
        bool IsTweetsDownloading,
        int GroupId
    )
    {
        public User() : this(default, string.Empty, string.Empty, default, default) { }
    }

    public record MenuItems(Dictionary<int, Group> Groups, Dictionary<int, User> Users, Dictionary<int, List<int>> GroupUsers)
    {
        public MenuItems() : this(new Dictionary<int, Group>(), new Dictionary<int, User>(), new Dictionary<int, List<int>>()) { }
    }
}
