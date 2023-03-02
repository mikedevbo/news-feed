using Dapper;
using System.Data;

namespace NewsFeed.Server.Twitter
{
    public static class TwitterCommon
    {
        public static async Task<List<int>> SetUserIsDownloadingTweetsState(
            IDbConnection connection,
            IDbTransaction transaction,
            List<int> userIds,
            bool state)
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
    }
}
