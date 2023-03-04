using NewsFeed.Server.Twitter.ExternalApi;
using NewsFeed.Server.Twitter.Messaging.DownloadTweetsPolicy.Commands;
using NServiceBus;

namespace NewsFeed.Server.Twitter.Messaging.DownloadTweetsPolicy
{
    namespace Commands
    {
        public record StartDownloadingTweets(List<(int UserId, string TwitterUserId)> Users);

        public record DownloadTweets(int UserId, string TwitterUserId);

        public record SaveTweetsData(int UserId, List<(string TweetId, string TweetText, DateTime? CreatedAt)> Tweets);

        public record SetUserTweetsDownloadingState(int UserId);
    }

    namespace Handlers
    {
        public class DownloadTweetsHandlers :
            IHandleMessages<StartDownloadingTweets>,
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

            public async Task Handle(StartDownloadingTweets message, IMessageHandlerContext context)
            {
                this.Log(message, context);

                foreach (var (UserId, TwitterUserId) in message.Users)
                {
                    await context.Send(new DownloadTweets(UserId, TwitterUserId));
                }
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
}