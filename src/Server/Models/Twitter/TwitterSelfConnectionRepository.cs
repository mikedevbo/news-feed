using Dapper.Contrib.Extensions;
using NewsFeed.Server.Models.Twitter.Tables;
using NewsFeed.Shared.Dto;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;

namespace NewsFeed.Server.Models.Twitter
{
    public class TwitterSelfConnectionRepository : ITwitterSelfConnectionRepository
    {
        private readonly IConfiguration configuration;

        public TwitterSelfConnectionRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<TwitterMenuResponse> GetTwitterMenu(int accountId)
        {
            //var sql = new StringBuilder();
            //sql.Append("select t.Id ");
            //sql.Append("from dbo.TwitterTweets t ");
            //sql.Append("inner join dbo.TwitterTweetsApi tapi on t.Id = tapi.Id ");
            //sql.Append("where t.UserId = @userId and t.IsPersisted = 0 and tapi.CreatedAt < @createdAt");

            //var ids = (await this.connection.QueryAsync<int>(
            //    sql.ToString(),
            //    new { userId, createdAt },
            //    this.transaction
            //)).ToList();

            throw new NotImplementedException();
        }

        public async Task SaveGroup(int accountId, string groupName)
        {
            using var connection = new SqlConnection(this.configuration.GetValue<string>(Constants.ConnectionStringPersistenceKey));
            await connection.OpenAsync();

            await connection.InsertAsync(
                new TwitterGroups
                {
                    Name = groupName,
                    AccountId = accountId
                }
            );
        }

        public async Task SaveUser(string userName, int groupId, string twitterUserId)
        {
            using var connection = new SqlConnection(this.configuration.GetValue<string>(Constants.ConnectionStringPersistenceKey));
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            try
            {
                var userId = await connection.InsertAsync(
                    new TwitterUsers
                    {
                        Name = userName,
                        GroupId = groupId,
                        IsTweetsDownloading = false
                    },
                    transaction
                );

                await connection.InsertAsync(
                    new TwitterUsersApi
                    {
                        Id = userId,
                        UserId = twitterUserId
                    },
                    transaction
                );

                transaction.Commit();
            }
            catch(Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
