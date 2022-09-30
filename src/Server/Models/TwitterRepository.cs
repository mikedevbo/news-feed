using NewsFeed.Shared;
using TwitterSharp.Response.RTweet;

namespace NewsFeed.Server.Models
{
    public class TwitterRepository : ITwitterRepository
    {
        public Task SaveTweets(int UserId, List<Tweet> Tweets)
        {
            throw new NotImplementedException();
        }

        public IList<TweetDto> GetDownloadedTweets(string accountId)
        {
            return new List<TweetDto>();
        }
    }
}