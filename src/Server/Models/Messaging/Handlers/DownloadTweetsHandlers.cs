using NewsFeed.Server.Models.Messaging.Commands;
using NewsFeed.Server.Models.Twitter;
using NServiceBus;

namespace NewsFeed.Server.Models.Messaging.Handlers
{
    public class DownloadTweetsHandlers :
        IHandleMessages<DownloadTweets>,
        IHandleMessages<SaveTweetsAndMarkAsDownloaded>,
        IHandleMessages<ClearOldTweets>
    {
        private readonly ITwitterApiClient twitterApiClient;
        private readonly ITwitterRepository twitterRepository;

        public DownloadTweetsHandlers(
            ITwitterApiClient twitterApiClient,
            ITwitterRepository twitterRepository)
        {
            this.twitterApiClient = twitterApiClient;
            this.twitterRepository = twitterRepository;
        }

        public async Task Handle(DownloadTweets message, IMessageHandlerContext context)
        {
            var tweets = await this.twitterApiClient.GetTweets(message.TwitterUserId);
            var command = new SaveTweetsAndMarkAsDownloaded(message.UserId, tweets);
            await context.Send(command);
        }

        public async Task Handle(SaveTweetsAndMarkAsDownloaded message, IMessageHandlerContext context)
        {
            //TODO: save tweets
            var command = new ClearOldTweets(message.UserId);
            await context.Send(command);
        }

        public Task Handle(ClearOldTweets message, IMessageHandlerContext context)
        {
            //TODO: clear tweets
            return Task.CompletedTask;
        }
    }
}
