using NewsFeed.Server.Twitter.ExternalApi;
using NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga.Commands;
using NServiceBus;

namespace NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga.Commands
{
    public record StartDownloadingTweets(List<(int UserId, string TwitterUserId)> Users);

    public record StartDownloadingTweetsForUser(int UserId, string TwitterUserId);

    public record DownloadTweets(int UserId, string TwitterUserId);

    public record SaveTweetsData(int UserId, List<(string TweetId, string TweetText, DateTime? CreatedAt)> Tweets);

    public record SetUserTweetsDownloadingState(int UserId);
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
                await Console.Out.WriteLineAsync($"{UserId} {TwitterUserId}");
                // await context.Send(new StartDownloadingTweetsForUser(UserId, TwitterUserId));
            }
        }
    }
}

namespace NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga.Handlers
{
    public class DownloadTweetsHandlers :
        IHandleMessages<DownloadTweets>,
        IHandleMessages<SaveTweetsData>,
        IHandleMessages<SetUserTweetsDownloadingState>
    {
        private readonly ILogger logger;
        private readonly ITwitterApiClient twitterApiClient;

        public DownloadTweetsHandlers(
            ILogger<DownloadTweetsHandlers> logger,
            ITwitterApiClient twitterApiClient)
        {
            this.logger = logger;
            this.twitterApiClient = twitterApiClient;
        }

        public async Task Handle(DownloadTweets message, IMessageHandlerContext context)
        {
            this.Log(message, context);

            var tweets = await twitterApiClient.GetTweets(message.TwitterUserId);
            var tweetsData = tweets.Select(t => (t.Id, t.Text, t.CreatedAt)).ToList();

            await context.Send(new SaveTweetsData(message.UserId, tweetsData));
        }

        public async Task Handle(SaveTweetsData message, IMessageHandlerContext context)
        {
            this.Log(message, context);

            ////TODO: save tweets data and get ids

            await context.Send(new SetUserTweetsDownloadingState(message.UserId));
        }

        public async Task Handle(SetUserTweetsDownloadingState message, IMessageHandlerContext context)
        {
            this.Log(message, context);

            var session = context.SynchronizedStorageSession.SqlPersistenceSession();

            await TwitterCommon.SetUserIsDownloadingTweetsState(
                session.Connection,
                session.Transaction,
                new List<int> { message.UserId },
                false);
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