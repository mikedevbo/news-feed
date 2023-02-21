using MediatR;

namespace NewsFeed.Shared.Twitter.Contracts
{
    public class GetMenuRequest : IRequest<string>
    {
        public int AccountId { get; set; }
    }
}
