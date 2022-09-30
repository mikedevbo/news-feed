using NewsFeed.Shared;
using TwitterSharp.Response.RTweet;

namespace NewsFeed.Server.Models
{
    public interface ITwitterRepository
    {
        Task SaveTweets(int UserId, List<Tweet> Tweets);

        IList<TweetDto> GetDownloadedTweets(string accountId);
    }
}
