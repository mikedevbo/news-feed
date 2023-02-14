using static NewsFeed.Shared.Twitter.Commands.StartDownloadingTweets;

namespace NewsFeed.Shared.Twitter.Commands
{
    public record StartDownloadingTweets(List<UserData> Users)
    {
        public record UserData(int UserId, string TwitterUserId);
    }

    public record SetReadState(int TweetId, bool IsRead);

    public record SetFavoriteState(int TweetId, bool IsPersisted);
}
