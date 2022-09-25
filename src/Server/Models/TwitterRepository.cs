using NewsFeed.Shared;
using TwitterSharp.Request.AdvancedSearch;
using TwitterSharp.Request.Option;

namespace NewsFeed.Server.Models
{
    public class TwitterRepository : ITwitterRepository
    {
        public IList<TweetDto> GetDownloadedTweets(string accountId)
        {
            return new List<TweetDto>();
        }
    }
}