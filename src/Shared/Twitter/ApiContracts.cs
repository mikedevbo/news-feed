using MediatR;

namespace NewsFeed.Shared.Twitter
{
    public record GetMenuRequest(int AccountId) : IRequest<string>;

    public record GetTweetsRequest(int UserId) : IRequest<string>;

    public record SetReadStateRequest(int TweetId, bool IsRead) : IRequest;

    public record SetFavoriteStateRequest(int TweetId, bool IsFavorite) : IRequest;

    public record StartDownloadingTweetsRequest(List<StartDownloadingTweetsRequest.UserData> Users) : IRequest
    {
        public record UserData(int UserId, string TwitterUserId);
    }
}