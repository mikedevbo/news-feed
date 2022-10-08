using NewsFeed.Shared.Dto;

namespace NewsFeed.Server.Models.Twitter
{
    public interface ITwitterSelfConnectionRepository
    {
        Task<TwitterMenuResponse> GetMenu(int accountId);

        Task<GroupResponse> SaveGroup(int accountId, string groupName);

        Task<UserResponse> SaveUser(string userName, int groupId, string twitterUserId);
    }
}
