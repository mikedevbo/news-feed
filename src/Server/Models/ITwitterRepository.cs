using NewsFeed.Shared;

namespace NewsFeed.Server.Models
{
    public interface ITwitterRepository
    {
        IList<Tweet> GetDownloadedTweets(string accountId);
    }
}
