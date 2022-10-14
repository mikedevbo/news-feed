using Dapper.Contrib.Extensions;

namespace NewsFeed.Server.Models.Twitter.Entity
{
    [Table("TwitterUsersApi")]
    public record TwitterUsersApi([property: ExplicitKey] int Id, string UserId)
    {
        public TwitterUsersApi() : this(default, string.Empty) { }

        public TwitterUsersApi(string UserId) : this(default, UserId) { }
    }
}
