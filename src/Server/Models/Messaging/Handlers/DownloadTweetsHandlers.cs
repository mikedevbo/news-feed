using NewsFeed.Server.Models.Messaging.Commands;
using NewsFeed.Server.Models.Twitter;
using NServiceBus;

namespace NewsFeed.Server.Models.Messaging.Handlers
{
    public class DownloadTweetsHandlers :
        IHandleMessages<DownloadTweets>,
        IHandleMessages<SaveTweets>,
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
            this.Log(message, context);

            var tweets = await this.twitterApiClient.GetTweets(message.TwitterUserId);

            var command = new SaveTweets(message.UserId, tweets);
            await context.Send(command);
        }

        public async Task Handle(SaveTweets message, IMessageHandlerContext context)
        {
            this.Log(message, context);

            await this.twitterRepository.SaveTweets(message.UserId, message.Tweets);
            await this.twitterRepository.SetTweetsDownloadingState(message.UserId, false);

            var command = new ClearOldTweets(message.UserId);
            await context.Send(command);
        }

        public async Task Handle(ClearOldTweets message, IMessageHandlerContext context)
        {
            this.Log(message, context);

            await this.twitterRepository.ClearOldTweets(message.UserId, DateTime.Now.AddMonths(-1));
        }

        private void Log<TMessage>(
            TMessage message,
            IMessageHandlerContext context)
        {
            // var info = System.Text.Json.JsonSerializer.Serialize(message);
            var info = context.MessageId;
            this.logger.LogInformation($"{typeof(TMessage).Name} - {info} ");
        }
    }
}
