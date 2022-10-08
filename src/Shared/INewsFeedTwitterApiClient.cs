using NewsFeed.Shared.Dto;

namespace NewsFeed.Shared
{
    public interface INewsFeedTwitterApiClient
    {
        Task<TwitterMenuResponse> GetMenu(int accountId);

        Task<GroupResponse> AddGroup(int accountId, string groupName);

        Task<UserResponse> AddUser(string userName, int groupId);
    }
}
