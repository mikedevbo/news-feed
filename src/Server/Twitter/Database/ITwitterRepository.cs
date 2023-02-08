using NewsFeed.Shared;
using TwitterSharp.Response.RTweet;

namespace NewsFeed.Server.Twitter.Database
{
    public interface ITwitterRepository
    {
        Task SaveTweets(int userId, List<Tweet> tweets);

        Task SetTweetsDownloadingState(int userId, bool isDownloading);

        Task<List<int>> SetUserIsDownloadingTweetsState(List<int> users, bool state);

        Task ClearOldTweets(int userId, DateTime createdAt);

        IList<TweetDto> GetDownloadedTweets(string accountId);
    }
}
