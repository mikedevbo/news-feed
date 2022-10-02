using NewsFeed.Shared.Dto;

namespace NewsFeed.Shared
{
    public interface INewsFeedApiClient
    {
        Task<TwitterMenuResponse> GetTwitterMenu();

        Task AddTwitterGroup(int accountId, string groupName);

        Task AddUser(string userName, int groupId);
    }
}
