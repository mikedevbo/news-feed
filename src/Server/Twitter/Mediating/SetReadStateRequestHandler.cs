using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using NewsFeed.Shared.Twitter;
using System.Threading;

namespace NewsFeed.Server.Twitter.Mediating
{
    public class SetReadStateRequestHandler : IRequestHandler<SetReadStateRequest>
    {
        private readonly IConfiguration configuration;

        public SetReadStateRequestHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task Handle(SetReadStateRequest request, CancellationToken cancellationToken)
        {
            return this.Command(request);
        }

        public async Task Command(SetReadStateRequest request)
        {
            var sql = @"UPDATE [dbo].[TwitterTweets]
SET IsRead = @IsRead
WHERE Id in @TweetIds";

            using var connection = new SqlConnection(configuration.GetValue<string>(Constants.ConnectionStringPersistenceKey));
            await connection.OpenAsync();

            var groupId = await connection.ExecuteAsync(sql, new { request.TweetIds, request.IsRead });
        }
    }
}
