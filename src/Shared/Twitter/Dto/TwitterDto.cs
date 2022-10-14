namespace NewsFeed.Shared.Twitter.Dto
{
    public record Group(
        int Id,
        string Name
    );

    public record User(
        int Id,
        string Name,
        string TwitterUserId,
        bool IsTweetsDownloading,
        int GroupId
    );

    public record MenuItems(Dictionary<int, Group> Groups, Dictionary<int, User> Users, Dictionary<int, List<int>> GroupUsers)
    {
        public MenuItems() : this(new Dictionary<int, Group>(), new Dictionary<int, User>(), new Dictionary<int, List<int>>()) { }
    }
}
