using static NewsFeed.Shared.Dto.TwitterMenuResponse;

namespace NewsFeed.Shared.Dto
{
    public record TwitterMenuResponse(List<GroupResponse> Groups)
    {
        public TwitterMenuResponse() : this(new List<GroupResponse>()) { }
    }

    public record GroupResponse(
        int Id,
        string Name,
        List<UserResponse> Users)
    {
        public GroupResponse() : this(default, string.Empty, new List<UserResponse>()) { }
    }

    public record UserResponse(
        int Id,
        string Name,
        string TwitterUserId,
        bool IsTweetsDownloading,
        int GroupId)
    {
        public UserResponse() : this(default, string.Empty, string.Empty, default, default) { }
    }
}
