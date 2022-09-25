using TwitterSharp.Client;
using TwitterSharp.Request.AdvancedSearch;
using TwitterSharp.Request.Option;
using TwitterSharp.Response.RTweet;
using TwitterSharp.Response.RUser;

namespace NewsFeed.Server.Models.Twitter
{
    public class TwitterApiClient : ITwitterApiClient
    {
        private readonly TwitterClient client;

        public TwitterApiClient(string token)
        {
            this.client = new TwitterClient(token);
        }

        public async Task<User> GetUser(string userName)
        {
            return await client.GetUserAsync(userName);
        }

        public async Task<List<Tweet>> GetTweets(string userId)
        {
            var tweets = await client.GetTweetsFromUserIdAsync(
                userId,
                new TweetSearchOptions
                {
                    TweetOptions = new[] { TweetOption.Created_At }
                }
            );

            return tweets.OrderByDescending(t => t.CreatedAt).ToList();
        }
    }
}