using NewsFeed.Server.Twitter.Database;
using NewsFeed.Server.Twitter.ExternalApi;
using NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga.Commands;
using NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga.Messages;
using NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga.SagaData;
using NServiceBus;

namespace NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga.Commands
{
    public record StartDownloadingTweets(List<StartDownloadingTweets.UserData> Users)
    {
        public record UserData(int UserId, string TwitterUserId);
    }

    public record StartDownloadingTweetsForUser(int UserId, string TwitterUserId);

    public record DownloadTweets(int UserId, string TwitterUserId);

    public record ClearOldTweets(int UserId);
}

namespace NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga.Messages
{
    public record ClearOldTweetsTimeout();
}

namespace NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga.SagaData
{
    public class DownloadTweetsSagaData : ContainSagaData
    {
        public int UserId { get; set; }

        public bool IsInProgress { get; set; }
    }
}

namespace NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga.EntryPointHandler
{
    public class StartDownloadingTweetsHandler :
        IHandleMessages<StartDownloadingTweets>
    {
        public async Task Handle(StartDownloadingTweets message, IMessageHandlerContext context)
        {
            foreach (var (UserId, TwitterUserId) in message.Users)
            {
                await context.Send(new StartDownloadingTweetsForUser(UserId, TwitterUserId));
            }
        }
    }
}

namespace NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga
{
    public class DownloadTweetsSaga :
        Saga<DownloadTweetsSagaData>,
        IAmStartedByMessages<StartDownloadingTweetsForUser>,
        IHandleTimeouts<ClearOldTweetsTimeout>
    {
        public async Task Handle(StartDownloadingTweetsForUser message, IMessageHandlerContext context)
        {
            if (!this.Data.IsInProgress)
            {
                this.Data.IsInProgress = true;
                await this.RequestClearOldTweetsTimeout(context);
            }

            await context.Send(new DownloadTweets(message.UserId, message.TwitterUserId));
        }

        public async Task Timeout(ClearOldTweetsTimeout state, IMessageHandlerContext context)
        {
            await context.Send(new ClearOldTweets(this.Data.UserId));
            await this.RequestClearOldTweetsTimeout(context);
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<DownloadTweetsSagaData> mapper)
        {
            mapper.MapSaga(s => s.UserId)
                  .ToMessage<StartDownloadingTweetsForUser>(m => m.UserId);
        }

        private async Task RequestClearOldTweetsTimeout(IMessageHandlerContext context)
        {
            await this.RequestTimeout<ClearOldTweetsTimeout>(context, TimeSpan.FromDays(7));
        }
    }
}

namespace NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga.Handlers
{
    public class DownloadTweetsHandlers :
        IHandleMessages<DownloadTweets>,
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
            Log(message, context);

            var tweets = await twitterApiClient.GetTweets(message.TwitterUserId);

            ////TODO Best to split into separate handlers == shorter transactions
            await twitterRepository.SaveTweets(message.UserId, tweets);
            await twitterRepository.SetTweetsDownloadingState(message.UserId, false);
        }

        public Task Handle(ClearOldTweets message, IMessageHandlerContext context)
        {
            ////TODO: add logic
            return Task.CompletedTask;
        }

        private void Log<TMessage>(
            TMessage message,
            IMessageHandlerContext context)
        {
#if DEBUG
            // var info = System.Text.Json.JsonSerializer.Serialize(message);
            var info = context.MessageId;
            logger.LogInformation($"{typeof(TMessage).Name} - {info} ");
#endif
        }
    }
}