using NewsFeed.Server.Twitter.Messaging.ClearTweetsPolicy.Commands;
using NewsFeed.Server.Twitter.Messaging.ClearTweetsPolicy.Messages;
using NewsFeed.Server.Twitter.Messaging.ClearTweetsPolicy.SagaData;
using NServiceBus;

namespace NewsFeed.Server.Twitter.Messaging.ClearTweetsPolicy
{
    namespace Commands
    {
        public record InitializeClearingTweets(int InstanceId);

        public record ClearTweets();
    }
    
    namespace Messages
    {
        public record ClearTweetsTimeout();
    }

    namespace SagaData
    {
        public class ClearTweetsSagaData : ContainSagaData
        {
            public int InstanceId { get; set; }

            public bool IsInProgress { get; set; }
        }
    }

    namespace Saga
    {
        public class ClearTweetsSaga :
            Saga<ClearTweetsSagaData>,
            IAmStartedByMessages<InitializeClearingTweets>,
            IHandleTimeouts<ClearTweetsTimeout>
        {
            public async Task Handle(InitializeClearingTweets message, IMessageHandlerContext context)
            {
                if (this.Data.IsInProgress)
                {
                    return;
                }

                this.Data.IsInProgress = true;
                await this.RequestClearOldTweetsTimeout(context);
            }

            public async Task Timeout(ClearTweetsTimeout state, IMessageHandlerContext context)
            {
                await context.Send(new ClearTweets());
                await this.RequestClearOldTweetsTimeout(context);
            }

            protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ClearTweetsSagaData> mapper)
            {
                mapper.MapSaga(s => s.InstanceId)
                      .ToMessage<InitializeClearingTweets>(m => m.InstanceId);
            }

            private async Task RequestClearOldTweetsTimeout(IMessageHandlerContext context)
            {
                await this.RequestTimeout<ClearTweetsTimeout>(context, TimeSpan.FromDays(7));
            }
        }
    }

    namespace Handlers
    {
        public class DownloadTweetsHandlers :
            IHandleMessages<ClearTweets>
        {
            private readonly ILogger logger;

            public DownloadTweetsHandlers(
                ILogger<DownloadTweetsHandlers> logger)
            {
                this.logger = logger;
            }

            public Task Handle(ClearTweets message, IMessageHandlerContext context)
            {
                this.Log(message, context);
                
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
}
