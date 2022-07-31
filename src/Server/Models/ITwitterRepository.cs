using NewsFeed.Shared;

namespace NewsFeed.Server.Models
{
    public interface ITwitterRepository
    {
        IList<TweetDto> GetDownloadedTweets(string accountId);
    }
}
