using NewsFeed.Shared.Twitter.Dto;
using NewsFeed.Shared.Twitter.Model;

namespace NewsFeed.Shared
{
    public interface INewsFeedTwitterApiClient
    {
        Task<MenuItems> GetMenu(int accountId);

        Task<Group> AddGroup(int accountId, string groupName);

        //Task<UserResponse> AddUser(string userName, int groupId);
    }
}
