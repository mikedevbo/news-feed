using TwitterSharp.Response.RTweet;
using TwitterSharp.Response.RUser;

namespace NewsFeed.Server.Models.Twitter
{
    public interface ITwitterApiClient
    {
        Task<User> GetUser(string userName);

        Task<List<Tweet>> GetTweets(string userId);
    }
}
