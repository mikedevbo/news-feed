using NewsFeed.Shared.Dto;
using NewsFeed.Shared.Twitter.Dto;

namespace NewsFeed.Shared
{
    public interface INewsFeedTwitterApiClient
    {
        Task<MenuItems> GetMenu(int accountId);

        //Task<GroupResponse> AddGroup(int accountId, string groupName);

        //Task<UserResponse> AddUser(string userName, int groupId);
    }
}
