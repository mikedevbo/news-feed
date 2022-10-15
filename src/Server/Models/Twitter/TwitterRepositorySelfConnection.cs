using Dapper;
using Dapper.Contrib.Extensions;
using NewsFeed.Server.Models.Twitter.Entity;
using NewsFeed.Shared.Twitter.Dto;
using NewsFeed.Shared.Twitter.Model;
using System.Data.SqlClient;

namespace NewsFeed.Server.Models.Twitter
{
    public class TwitterRepositorySelfConnection : ITwitterRepositorySelfConnection
    {
        private readonly IConfiguration configuration;

        public TwitterRepositorySelfConnection(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<MenuItems> GetMenu(int accountId)
        {
            var menuItems = new MenuItems(
                new Dictionary<int, Group>(),
                new Dictionary<int, User>(),
                new Dictionary<int, List<int>>()
            );

            var sql = @"
                select g.Id, g.Name, u.Id, u.Name, u.IsTweetsDownloading, u.GroupId, uapi.Id, uapi.UserId
                from dbo.TwitterGroups g
                left join dbo.TwitterUsers u on g.Id = u.GroupId
                left join dbo.TwitterUsersApi uapi on u.Id = uapi.Id
                where g.AccountId = @accountId
            ";

            using var connection = new SqlConnection(this.configuration.GetValue<string>(Constants.ConnectionStringPersistenceKey));
            await connection.OpenAsync();

            await connection.QueryAsync<TwitterGroup?, TwitterUser?, TwitterUsersApi?, bool>(
                sql: sql.ToString(),
                map: (group, user, userApi) =>
                {
                    if (group is not null && !menuItems.Groups.ContainsKey(group.Id))
                    {
                        menuItems.Groups.Add(group.Id, new Group(group.Id, group.Name));
                        menuItems.GroupUsers.Add(group.Id, new List<int>());
                    }

                    if (user is not null)
                    {
                        menuItems.Users.Add(user.Id, new User(user.Id, user.Name, (userApi?.UserId)??string.Empty, user.IsTweetsDownloading, user.GroupId));

                        if (group is not null)
                        {
                            menuItems.GroupUsers[group.Id].Add(user.Id);
                        }
                    }

                    return true;
                },
                param: new { accountId }
            );

            return menuItems;
        }

        public async Task<Group> SaveGroup(TwitterGroup group)
        {
            using var connection = new SqlConnection(this.configuration.GetValue<string>(Constants.ConnectionStringPersistenceKey));
            await connection.OpenAsync();

            var groupId = await connection.InsertAsync(group);

            return new Group(groupId, group.Name);
        }

        public async Task<User> SaveUser(TwitterUser user, TwitterUsersApi userApi)
        {
            int userId;
            using var connection = new SqlConnection(this.configuration.GetValue<string>(Constants.ConnectionStringPersistenceKey));
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
    }
}
