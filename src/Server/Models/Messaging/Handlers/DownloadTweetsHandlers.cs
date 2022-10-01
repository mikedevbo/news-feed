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
        private readonly ILogger logger;
        private readonly ITwitterApiClient twitterApiClient;
        private readonly ITwitterRepository twitterRepository;

        public DownloadTweetsHandlers(
            ILogger<DownloadTweetsHandlers> logger,
            ITwitterApiClient twitterApiClient,
            ITwitterRepository twitterRepository)
        {
            this.logger = logger;
            this.twitterApiClient = twitterApiClient;
            this.twitterRepository = twitterRepository;
        }

        public async Task Handle(DownloadTweets message, IMessageHandlerContext context)
        {
            this.LogInfo(message);

            var tweets = await this.twitterApiClient.GetTweets(message.TwitterUserId);
            var command = new SaveTweetsAndMarkAsDownloaded(message.UserId, tweets);
            await context.Send(command);
        }

        public async Task Handle(SaveTweetsAndMarkAsDownloaded message, IMessageHandlerContext context)
        {
            this.LogInfo(message);

            //TODO: save tweets
            var command = new ClearOldTweets(message.UserId);
            await context.Send(command);
        }

        public Task Handle(ClearOldTweets message, IMessageHandlerContext context)
        {
            this.LogInfo(message);

            //TODO: clear tweets
            return Task.CompletedTask;
        }

        private void LogInfo<TMessage>(TMessage message)
        {
            this.logger.LogInformation($"{typeof(TMessage).Name} - {System.Text.Json.JsonSerializer.Serialize(message)}");
        }
    }
}
