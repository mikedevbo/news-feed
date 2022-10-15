using System.Security.Policy;

namespace NewsFeed.Server.Models.Twitter.Entity
{
    public record TwitterUser(int Id, string Name, int GroupId, bool IsTweetsDownloading)
    {
        public TwitterUser() : this(default, string.Empty, default, default) { }

        public TwitterUser(string Name, int GroupId) : this(default, Name, GroupId, default) { }
    }
}
