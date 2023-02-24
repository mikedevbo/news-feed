using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using NewsFeed.Shared.Twitter;

namespace NewsFeed.Server.Twitter.Mediating
{
    public class SetFavoriteStateRequestHandler : IRequestHandler<SetFavoriteStateRequest>
    {
        private readonly IConfiguration configuration;

        public SetFavoriteStateRequestHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task Handle(SetFavoriteStateRequest request, CancellationToken cancellationToken)
        {
            return this.Command(request);
        }

        public async Task Command(SetFavoriteStateRequest request)
        {
            var sql = @"UPDATE [dbo].[TwitterTweets]
SET IsFavorite = @IsFavorite
WHERE Id = @tweetId";

            using var connection = new SqlConnection(configuration.GetValue<string>(Constants.ConnectionStringPersistenceKey));
            await connection.OpenAsync();

            var groupId = await connection.ExecuteAsync(sql, new { request.TweetId, request.IsFavorite });
        }
    }
}
