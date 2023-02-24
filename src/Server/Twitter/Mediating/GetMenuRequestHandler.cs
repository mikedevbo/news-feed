using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using NewsFeed.Shared.Twitter;

namespace NewsFeed.Server.Twitter.Mediating
{
    public class GetMenuRequestHandler : IRequestHandler<GetMenuRequest, string>
    {
        private readonly IConfiguration configuration;

        public GetMenuRequestHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<string> Handle(GetMenuRequest request, CancellationToken cancellationToken)
        {
            return this.Query(request);
        }

        public async Task<string> Query(GetMenuRequest request)
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
where [Group].AccountId = @AccountId
for xml auto, elements, root('Root')";

            using var connection = new SqlConnection(configuration.GetValue<string>(Constants.ConnectionStringPersistenceKey));
            await connection.OpenAsync();
            var data = await connection.QueryAsync<string>(sql, new { request.AccountId });
            return data.Any()
                ? string.Join("", data)
                : "<Root />";
        }
    }
}
