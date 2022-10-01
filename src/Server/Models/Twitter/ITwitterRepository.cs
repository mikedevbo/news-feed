using NewsFeed.Shared;
using TwitterSharp.Response.RTweet;

namespace NewsFeed.Server.Models.Twitter
{
    public interface ITwitterRepository
    {
        Task SaveTweets(int userId, List<Tweet> tweets);

        IList<TweetDto> GetDownloadedTweets(string accountId);
    }
}
