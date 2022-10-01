using NewsFeed.Shared;
using TwitterSharp.Response.RTweet;

namespace NewsFeed.Server.Models.Twitter
{
    public interface ITwitterRepository
    {
        Task SaveTweets(int userId, List<Tweet> tweets);

        Task SetTweetsDownloadingState(int userId, bool isDownloading);

        Task ClearOldTweets(int userId, DateTime createdAt);

        IList<TweetDto> GetDownloadedTweets(string accountId);
    }
}
