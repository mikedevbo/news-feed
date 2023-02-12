using NewsFeed.Server.Models.Twitter.Entity;
using NewsFeed.Shared.Twitter.Model;

namespace NewsFeed.Server.Twitter.Database
{
    public interface ITwitterRepositorySelfConnection
    {
        Task<string> GetMenu(int accountId);

        Task<Group> SaveGroup(TwitterGroup group);

        Task<User> SaveUser(TwitterUser user, TwitterUsersApi userApi);

        Task<List<Tweet>> GetTweets(int userId);

        Task SetTweetReadState(int tweetId, bool isRead);
    }
}
