using NewsFeed.Shared.Twitter.Dto;
using NewsFeed.Shared.Twitter.Model;

namespace NewsFeed.Shared.Twitter
{
    public interface ITwitterNewsFeedApiClient
    {
        Task<MenuItems> GetMenu(int accountId);

        Task<Group> AddGroup(string groupName, int accountId);

        Task<User> AddUser(string userName, int groupId);
    }
}
