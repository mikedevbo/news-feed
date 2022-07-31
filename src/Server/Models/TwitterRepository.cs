using NewsFeed.Shared;
using System.Text.Json;

namespace NewsFeed.Server.Models
{
    public class TwitterRepository : ITwitterRepository
    {
        private readonly string basePath; 
        public TwitterRepository(string basePath)
        {
            this.basePath = basePath;
        }
        
        public IList<Tweet> GetDownloadedTweets(string accountId)
        {
            //var empty = new List<Tweet>();
            return new List<Tweet>();
        }

        private string CreateFilePath(string accountId)
        {
            return $"{this.basePath}{Path.DirectorySeparatorChar}{accountId}.db";
        }
    }
}