using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using NewsFeed.Shared.Twitter;

namespace NewsFeed.Server.Twitter.Mediating
{
    public class GetTweetsRequestHandler : IRequestHandler<GetTweetsRequest, string>
    {
        private readonly IConfiguration configuration;

        public GetTweetsRequestHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<string> Handle(GetTweetsRequest request, CancellationToken cancellationToken)
        {
            return this.Query(request);
        }

        public async Task<string> Query(GetTweetsRequest request)
        {
            if (request.UserId <= 0)
            {
                return "<Root />";
            }

            var sql = @"SELECT
tweet.[Id]
, tweet.[UserId]
, tweet.[IsFavorite]
, tweet.[IsRead]
, tweet.[TweetIdApi] as TweetId
, tweet.[TweetTextApi] as Text
, tweet.[TweetCreatedAtApi] as CreatedAt
FROM [dbo].[TwitterTweets] tweet
WHERE tweet.[UserId] = @UserId
for xml path ('Tweet'),root('Root')";

            using var connection = new SqlConnection(configuration.GetValue<string>(Constants.ConnectionStringPersistenceKey));
            await connection.OpenAsync();
            var data = await connection.QueryAsync<string>(sql, new { request.UserId });
            return data.Any()
                ? string.Join("", data)
                : "<Root />";
        }
    }
}
