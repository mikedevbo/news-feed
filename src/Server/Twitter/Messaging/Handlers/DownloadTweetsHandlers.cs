using NewsFeed.Server.Twitter.Database;
using NewsFeed.Server.Twitter.ExternalApi;
using NewsFeed.Server.Twitter.Messaging.Commands;
using NServiceBus;

namespace NewsFeed.Server.Twitter.Messaging.Handlers
{
    public class DownloadTweetsHandlers :
        IHandleMessages<DownloadTweetsRequests>,
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

        public async Task Handle(DownloadTweetsRequests message, IMessageHandlerContext context)
        {
            Log(message, context);

            var tweets = await twitterApiClient.GetTweets(message.TwitterUserId);

            var command = new SaveTweets(message.UserId, tweets);
            await context.Send(command);
        }

        public async Task Handle(SaveTweets message, IMessageHandlerContext context)
        {
            Log(message, context);

            await twitterRepository.SaveTweets(message.UserId, message.Tweets);
            await twitterRepository.SetTweetsDownloadingState(message.UserId, false);

            var command = new ClearOldTweets(message.UserId);
            await context.Send(command);
        }

        public async Task Handle(ClearOldTweets message, IMessageHandlerContext context)
        {
            Log(message, context);

            await twitterRepository.ClearOldTweets(message.UserId, DateTime.Now.AddMonths(-1));
        }

        private void Log<TMessage>(
            TMessage message,
            IMessageHandlerContext context)
        {
            // var info = System.Text.Json.JsonSerializer.Serialize(message);
            var info = context.MessageId;
            logger.LogInformation($"{typeof(TMessage).Name} - {info} ");
        }
    }
}
