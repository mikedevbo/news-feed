﻿using Dapper;
using Dapper.Contrib.Extensions;
using NewsFeed.Server.Models.Twitter.Tables;
using NewsFeed.Shared.Dto;
using System.Data.SqlClient;

namespace NewsFeed.Server.Models.Twitter
{
    public class TwitterSelfConnectionRepository : ITwitterSelfConnectionRepository
    {
        private readonly IConfiguration configuration;

        public TwitterSelfConnectionRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<TwitterMenuResponse> GetMenu(int accountId)
        {
            var result = new TwitterMenuResponse(new List<TwitterMenuResponse.Group>());

            var sql = @"
                select g.Id, g.Name, u.Id, u.Name, u.IsTweetsDownloading, uapi.UserId as TwitterUserId
                from dbo.TwitterGroups g
                left join dbo.TwitterUsers u on g.Id = u.GroupId
                left join dbo.TwitterUsersApi uapi on u.Id = uapi.Id
                where g.AccountId = @accountId
            ";

            static void AddUser(TwitterMenuResponse.User user, TwitterMenuResponse.Group group)
            {
                if (user is not null)
                {
                    group.Users.Add(user with { GroupId = group.Id });
                }
            }

            using var connection = new SqlConnection(this.configuration.GetValue<string>(Constants.ConnectionStringPersistenceKey));
            await connection.OpenAsync();

            await connection. QueryAsync<TwitterMenuResponse.Group, TwitterMenuResponse.User, int>(
                sql: sql.ToString(),
                map: (group, user) =>
                {
                    var groupIndex = result.Groups.FindIndex(g => g.Id == group.Id);
                    if (groupIndex == -1)
                    {
                        AddUser(user, group);
                        result.Groups.Add(group);
                    }
                    else
                    {
                        AddUser(user, result.Groups[groupIndex]);
                    }

                    return 0;
                },
                param: new { accountId }
            );

            return result;
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
