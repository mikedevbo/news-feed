using NewsFeed.Shared.Dto;

namespace NewsFeed.Shared
{
    public interface INewsFeedTwitterApiClient
    {
        Task<TwitterMenuResponse> GetMenu(int accountId);

        Task<GroupResponse> AddGroup(int accountId, string groupName);

        Task AddUser(string userName, int groupId);
    }
}
