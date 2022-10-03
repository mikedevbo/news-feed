using static NewsFeed.Shared.Dto.TwitterMenuResponse;

namespace NewsFeed.Shared.Dto
{
    public record TwitterMenuResponse(List<Group> Groups)
    {
        public record Group(
            int Id,
            string Name,
            List<User> Users)
        {
            public Group() : this(default, string.Empty, new List<User>()) { }
        }

        public record User(
            int Id,
            string Name,
            string TwitterUserId,
            bool IsTweetsDownloading,
            bool IsSelected,
            int GroupId)
        {
            public User() : this(default, string.Empty, string.Empty, default, default, default) { }
        }
    }
}
