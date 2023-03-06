using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using NewsFeed.Shared.Twitter;

namespace NewsFeed.Server.Twitter.Mediating
{
    public class GetDownloadingTweetsStateHandler : IRequestHandler<GetDownloadingTweetsState, string>
    {
        private readonly IConfiguration configuration;

        public GetDownloadingTweetsStateHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<string> Handle(GetDownloadingTweetsState request, CancellationToken cancellationToken)
        {
            return this.Query(request);
        }

        public async Task<string> Query(GetDownloadingTweetsState request)
        {
            if (request.UsersIds.Count <= 0)
            {
                return "<Root />";
            }

            var sql = @"select u.Id as UserId, u.IsTweetsDownloading, tweet.UnreadTweetsCount
from dbo.TwitterUsers u
left join
	(
		select t.UserId, count(*) as UnreadTweetsCount
		from dbo.TwitterTweets t
		where t.IsRead = 0
		group by t.UserId
	) as tweet on tweet.UserId = u.Id
where u.Id in @UsersIds
for xml path ('UserDownladTweetsState'), root('Root')";

            using var connection = new SqlConnection(configuration.GetValue<string>(Constants.ConnectionStringPersistenceKey));
            await connection.OpenAsync();
            var data = await connection.QueryAsync<string>(sql, new { request.UsersIds });
            return data.Any()
                ? string.Join("", data)
                : "<Root />";
        }
    }
}
