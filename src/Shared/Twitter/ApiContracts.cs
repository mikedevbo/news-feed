using MediatR;

namespace NewsFeed.Shared.Twitter
{
    public record GetMenuRequest(int AccountId) : IRequest<string>;

    public record GetTweetsRequest(int UserId) : IRequest<string>;
}