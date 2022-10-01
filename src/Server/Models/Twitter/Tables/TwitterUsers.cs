using Dapper.Contrib.Extensions;

namespace NewsFeed.Server.Models.Twitter.Tables
{
    [Table("TwitterUsers")]
    public class TwitterUsers
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public bool IsTweetsDownloading { get; set; }
    }
}
