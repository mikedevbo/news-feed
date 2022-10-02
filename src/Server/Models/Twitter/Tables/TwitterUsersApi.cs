using Dapper.Contrib.Extensions;

namespace NewsFeed.Server.Models.Twitter.Tables
{
    [Table("TwitterUsersApi")]
    public class TwitterUsersApi
    {
        [ExplicitKey]
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;
    }
}
