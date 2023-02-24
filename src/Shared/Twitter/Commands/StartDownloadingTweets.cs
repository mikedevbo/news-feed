using static NewsFeed.Shared.Twitter.Commands.StartDownloadingTweets;

namespace NewsFeed.Shared.Twitter.Commands
{
    public record StartDownloadingTweets(List<UserData> Users)
    {
        public record UserData(int UserId, string TwitterUserId);
    }
}
