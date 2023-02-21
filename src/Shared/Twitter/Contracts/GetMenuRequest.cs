using MediatR;

namespace NewsFeed.Shared.Twitter.Contracts
{
    public record GetMenuRequest(int AccountId) : IRequest<string>;
}
