using Dapper.Contrib.Extensions;

namespace NewsFeed.Server.Models.Twitter.Tables
{
    public class TwitterGroup
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int AccountId { get; set; }
    }
}
