using Dapper;
using Dapper.Contrib.Extensions;
using NewsFeed.Server.Models.Twitter.Entity;
using NewsFeed.Shared;
using System.Data.Common;
using System.Text;
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
                    new TwitterTweet
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

        public async Task ClearOldTweets(int userId, DateTime createdAt)
        {
            var sql = new StringBuilder();
            sql.Append("select t.Id ");
            sql.Append("from dbo.TwitterTweets t ");
            sql.Append("inner join dbo.TwitterTweetsApi tapi on t.Id = tapi.Id ");
            sql.Append("where t.UserId = @userId and t.IsPersisted = 0 and tapi.CreatedAt < @createdAt");

            var ids = (await this.connection.QueryAsync<int>(
                sql.ToString(),
                new { userId, createdAt },
                this.transaction
            )).ToList();

            var tweets = new List<TwitterTweet>();
            var tweetsApi = new List<TwitterTweetsApi>();
            ids.ForEach(id =>
            {
                tweets.Add(new TwitterTweet(id, 0, false));
                tweetsApi.Add(new TwitterTweetsApi(id, string.Empty, string.Empty, default));
            });


            await this.connection.DeleteAsync(tweets, this.transaction);
            await this.connection.DeleteAsync(tweetsApi, this.transaction);
        }

        public IList<TweetDto> GetDownloadedTweets(string accountId)
        {
            return new List<TweetDto>();
        }
    }
}