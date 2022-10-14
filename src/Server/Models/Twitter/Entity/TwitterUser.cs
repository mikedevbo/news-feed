using Dapper.Contrib.Extensions;

namespace NewsFeed.Server.Models.Twitter.Entity
{
    public class TwitterUser
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int GroupId { get; set; }

        public bool IsTweetsDownloading { get; set; }
    }
}
