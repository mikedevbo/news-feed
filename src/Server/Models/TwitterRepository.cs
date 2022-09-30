using NewsFeed.Shared;
using System.Data.Common;
using TwitterSharp.Response.RTweet;

namespace NewsFeed.Server.Models
{
    public class TwitterRepository : ITwitterRepository
    {
        private readonly DbConnection connection;
        private readonly DbTransaction transaction;

        public TwitterRepository(DbConnection connection, DbTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;
        }

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