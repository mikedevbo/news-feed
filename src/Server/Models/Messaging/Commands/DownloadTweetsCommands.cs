using NServiceBus;
using TwitterSharp.Response.RTweet;

namespace NewsFeed.Server.Models.Messaging.Commands
{
    public record DownloadTweets(int UserId, string TwitterUserId) : ICommand;

    public record SaveTweets(int UserId, List<Tweet> Tweets) : ICommand;

    public record ClearOldTweets(int UserId) : ICommand;
}
