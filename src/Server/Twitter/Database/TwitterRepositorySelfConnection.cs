using Dapper;
using Dapper.Contrib.Extensions;
using NewsFeed.Server.Models.Twitter.Entity;
using NewsFeed.Shared.Twitter.Model;
using System.Data.SqlClient;
using System.Threading;

namespace NewsFeed.Server.Twitter.Database
{
    public class TwitterRepositorySelfConnection : ITwitterRepositorySelfConnection
    {
        private readonly IConfiguration configuration;

        public TwitterRepositorySelfConnection(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> GetMenu(int accountId)
        {
            var sql = @"select [Group].Id, [Group].Name
, (
	select u.Id, u.Name, u.IsTweetsDownloading, u.GroupId, uapi.UserId as [TwitterUserId], tweet.UnreadTweetsCount
	from dbo.TwitterUsers as [u]
	left join dbo.TwitterUsersApi as uapi on u.Id = uapi.Id
	left join
	(
		select t.UserId, count(*) as UnreadTweetsCount
		from dbo.TwitterTweets t
		where t.IsRead = 0
		group by t.UserId
	) as tweet on tweet.UserId = u.Id
	where [u].GroupId = [Group].Id
	for xml path ('User'), type
) [Users]
from dbo.TwitterGroups as [Group]
where [Group].AccountId = 1
for xml auto, elements, root('Root')";

            using var connection = new SqlConnection(configuration.GetValue<string>(Constants.ConnectionStringPersistenceKey));
            await connection.OpenAsync();
            var data = await connection.QueryAsync<string>(sql, new { accountId });
            return data.Any()
                ? string.Join("", data)
                : "<Root />";
        }

        public async Task<Group> SaveGroup(TwitterGroup group)
        {
            using var connection = new SqlConnection(configuration.GetValue<string>(Constants.ConnectionStringPersistenceKey));
            await connection.OpenAsync();

            var groupId = await connection.InsertAsync(group);

            return new Group(groupId, group.Name);
        }

        public async Task<User> SaveUser(TwitterUser user, TwitterUsersApi userApi)
        {
            int userId;
            using var connection = new SqlConnection(configuration.GetValue<string>(Constants.ConnectionStringPersistenceKey));
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            try
            {
                userId = await connection.InsertAsync(user, transaction);
                await connection.InsertAsync(userApi with { Id = userId }, transaction);

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

            return new User(userId, user.Name, userApi.UserId, false, user.GroupId);
        }

        public async Task<List<Tweet>> GetTweets(int userId)
        {
            if (userId <= 0)
            {
                return new List<Tweet>();
            }

            var sql = @"SELECT tweet.[Id], tweet.[UserId], tweet.[IsPersisted], tweet.[IsRead] , tweetApi.[TweetId], tweetApi.[Text] ,tweetApi.[CreatedAt]
FROM [dbo].[TwitterTweets] tweet
INNER JOIN [dbo].[TwitterTweetsApi] tweetApi ON tweet.Id = tweetApi.Id
WHERE tweet.[UserId] = @userId";

            using var connection = new SqlConnection(configuration.GetValue<string>(Constants.ConnectionStringPersistenceKey));
            await connection.OpenAsync();
            var data = await connection.QueryAsync<Tweet>(sql, new { userId});
            return data.OrderByDescending(t => t.CreatedAt).ToList();
        }

        public async Task SetTweetReadState(int tweetId, bool isRead)
        {
            var sql = @"UPDATE [dbo].[TwitterTweets]
SET IsRead = @isRead
WHERE Id = @tweetId";

            using var connection = new SqlConnection(configuration.GetValue<string>(Constants.ConnectionStringPersistenceKey));
            await connection.OpenAsync();

            var groupId = await connection.ExecuteAsync(sql, new { tweetId, isRead });
        }

        public async Task SetTweetPersistedState(int tweetId, bool isPersisted)
        {
            var sql = @"UPDATE [dbo].[TwitterTweets]
SET IsPersisted = @isPersisted
WHERE Id = @tweetId";

            using var connection = new SqlConnection(configuration.GetValue<string>(Constants.ConnectionStringPersistenceKey));
            await connection.OpenAsync();

            var groupId = await connection.ExecuteAsync(sql, new { tweetId, isPersisted });
        }
    }
}
