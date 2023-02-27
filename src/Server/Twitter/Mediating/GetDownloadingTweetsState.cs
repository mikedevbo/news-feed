using MediatR;
using NewsFeed.Shared.Twitter;

namespace NewsFeed.Server.Twitter.Mediating
{
    public class GetDownloadingTweetsStateHandler : IRequestHandler<GetDownloadingTweetsState, string>
    {
        private readonly IConfiguration configuration;

        public GetDownloadingTweetsStateHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<string> Handle(GetDownloadingTweetsState request, CancellationToken cancellationToken)
        {
            return this.Query(request);
        }

        public async Task<string> Query(GetDownloadingTweetsState request)
        {
            return @"<Root>
	<UserDownladTweetsState>
		<UserId>1</UserId>
		<IsDownloadingTweets>true</IsDownloadingTweets>
	</UserDownladTweetsState>
</Root>";
        }
    }
}
