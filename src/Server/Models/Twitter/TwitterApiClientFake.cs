using TwitterSharp.Client;
using TwitterSharp.Request.AdvancedSearch;
using TwitterSharp.Request.Option;
using TwitterSharp.Response.RTweet;
using TwitterSharp.Response.RUser;

namespace NewsFeed.Server.Models.Twitter
{
    public class TwitterApiClientFake : ITwitterApiClient
    {
        public Task<User> GetUser(string userName)
        {
            return Task.FromResult(new User { Id = "100" });
        }

        public Task<List<Tweet>> GetTweets(string userId)
        {
            var tweets = new List<Tweet>
            {
                new Tweet { Id = "1", CreatedAt = DateTime.Now, Text = "Test tweet 1" },
                new Tweet { Id = "2", CreatedAt = DateTime.Now.AddSeconds(20), Text = "Test tweet 2" },
                new Tweet { Id = "3", CreatedAt = DateTime.Now.AddSeconds(40), Text = "Test tweet 3" }
            };

            return Task.FromResult(tweets.OrderByDescending(t => t.CreatedAt).ToList());
        }
    }
}