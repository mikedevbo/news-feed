using NewsFeed.Shared.Dto;

namespace NewsFeed.Server.Models.Twitter
{
    public interface ITwitterSelfConnectionRepository
    {
        Task<TwitterMenuResponse> GetTwitterMenu(int accountId);

        Task SaveGroup(int accountId, string groupName);

        Task SaveUser(string userName, int groupId, string twitterUserId);
    }
}
