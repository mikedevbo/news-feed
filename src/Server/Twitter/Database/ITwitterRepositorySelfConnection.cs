using NewsFeed.Server.Models.Twitter.Entity;
using NewsFeed.Shared.Twitter.Dto;
using NewsFeed.Shared.Twitter.Model;

namespace NewsFeed.Server.Twitter.Database
{
    public interface ITwitterRepositorySelfConnection
    {
        Task<MenuItems> GetMenu(int accountId);

        Task<Group> SaveGroup(TwitterGroup group);

        Task<User> SaveUser(TwitterUser user, TwitterUsersApi userApi);

        Task<List<Tweet>> GetTweets(int userId);
    }
}
