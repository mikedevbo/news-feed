using Dapper;
using Dapper.Contrib.Extensions;
using NewsFeed.Server.Models.Twitter.Tables;
using NewsFeed.Shared;
using System.Data.Common;
using TwitterSharp.Response.RTweet;

namespace NewsFeed.Server.Models.Twitter
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

        public async Task SaveTweets(int userId, List<Tweet> tweets)
        {
            foreach(var tweet in tweets)
            {
                var tweetId = await this.connection.InsertAsync(
                    new TwitterTweets
                    {
                        UserId = userId,
                        IsPersisted = false
                    },
                    this.transaction
                );

                await this.connection.InsertAsync(
                    new TwitterTweetsApi
                    {
                        Id = tweetId,
                        TweetId = tweet.Id,
                        Text = tweet.Text,
                        CreatedAt = Convert.ToDateTime(tweet.CreatedAt)
                    },
                    this.transaction
                );
            }
        }

        public async Task SetTweetsDownloadingState(int userId, bool isTweetsDownloading)
        {
            const string sql = $"update dbo.TwitterUsers set IsTweetsDownloading = @value where Id = @userId";

            await this.connection.ExecuteAsync(
                sql,
                new { value = isTweetsDownloading, userId},
                this.transaction
            );
        }

        public IList<TweetDto> GetDownloadedTweets(string accountId)
        {
            return new List<TweetDto>();
        }
    }
}