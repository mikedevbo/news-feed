using Dapper;
using Dapper.Contrib.Extensions;
using NewsFeed.Server.Models.Twitter.Entity;
using NewsFeed.Shared;
using System.Data.Common;
using System.Text;
using TwitterSharp.Response.RTweet;

namespace NewsFeed.Server.Twitter.Database
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
            foreach (var tweet in tweets)
            {
                var tweetId = await connection.InsertAsync(
                    new TwitterTweet
                    {
                        UserId = userId,
                        IsPersisted = false,
                        IsRed = false
                    },
                    transaction
                );

                await connection.InsertAsync(
                    new TwitterTweetsApi
                    {
                        Id = tweetId,
                        TweetId = tweet.Id,
                        Text = tweet.Text,
                        CreatedAt = Convert.ToDateTime(tweet.CreatedAt)
                    },
                    transaction
                );
            }
        }

        public async Task SetTweetsDownloadingState(int userId, bool isTweetsDownloading)
        {
            const string sql = $"update dbo.TwitterUsers set IsTweetsDownloading = @value where Id = @userId";

            await connection.ExecuteAsync(
                sql,
                new { value = isTweetsDownloading, userId },
                transaction
            );
        }

        public async Task<List<int>> SetUserIsDownloadingTweetsState(List<int> userIds, bool state)
        {
            var sql = @"update dbo.TwitterUsers
set IsTweetsDownloading = @newState
output inserted.Id
where Id in @Ids and IsTweetsDownloading = @currentState";

            var result = await connection.QueryAsync<int>(
                sql,
                new { Ids = userIds, newState = state, currentState = !state },
                transaction
            );

            return result.ToList();
        }

        public async Task ClearOldTweets(int userId, DateTime createdAt)
        {
            var sql = new StringBuilder();
            sql.Append("select t.Id ");
            sql.Append("from dbo.TwitterTweets t ");
            sql.Append("inner join dbo.TwitterTweetsApi tapi on t.Id = tapi.Id ");
            sql.Append("where t.UserId = @userId and t.IsPersisted = 0 and tapi.CreatedAt < @createdAt");

            var ids = (await connection.QueryAsync<int>(
                sql.ToString(),
                new { userId, createdAt },
                transaction
            )).ToList();

            var tweets = new List<TwitterTweet>();
            var tweetsApi = new List<TwitterTweetsApi>();
            ids.ForEach(id =>
            {
                tweets.Add(new TwitterTweet(id, 0, false, false));
                tweetsApi.Add(new TwitterTweetsApi(id, string.Empty, string.Empty, default));
            });


            await connection.DeleteAsync(tweets, transaction);
            await connection.DeleteAsync(tweetsApi, transaction);
        }
    }
}