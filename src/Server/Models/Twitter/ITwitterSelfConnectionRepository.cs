using NewsFeed.Server.Models.Twitter.Entity;
using NewsFeed.Shared.Twitter.Dto;

namespace NewsFeed.Server.Models.Twitter
{
    public interface ITwitterSelfConnectionRepository
    {
        Task<MenuItems> GetMenu(int accountId);

        Task<Group> SaveGroup(TwitterGroup group);

        //Task<UserResponse> SaveUser(string userName, int groupId, string twitterUserId);
    }
}
