using NewsFeed.Server.Models.Messaging.Commands;
using NewsFeed.Server.Models.Messaging.Messages;
using NServiceBus;

namespace NewsFeed.Server.Models.Messaging.Sagas
{
    public class TweetsSaga
        : Saga<TweetsSagaData>,
        IAmStartedByMessages<DownloadNewTweets>,
        IAmStartedByMessages<GetDownloadedTweetsRequest>,
        IHandleMessages<DownloadNewTweetsFromTwitterApiResponse>
    {
        public Task Handle(DownloadNewTweets message, IMessageHandlerContext context)
        {
            return context.Send(new DownloadNewTweetsFromTwitterApiRequest
            {
                UserId = message.UserId
            });
        }

        public Task Handle(GetDownloadedTweetsRequest message, IMessageHandlerContext context)
        {
            return context.Reply(new GetDownloadedTweetsResponse { Data = this.Data });
        }

        public Task Handle(DownloadNewTweetsFromTwitterApiResponse message, IMessageHandlerContext context)
        {
            foreach(var tweet in message.Tweets)
            {
                this.Data.Tweets.Add(new TweetsSagaData.Tweet
                {
                    Text = tweet.Text
                });
            }

            return Task.CompletedTask;
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<TweetsSagaData> mapper)
        {
            mapper.MapSaga(saga => saga.UserId)
                .ToMessage<DownloadNewTweets>(message => message.UserId)
                .ToMessage<GetDownloadedTweetsRequest>(msg => msg.UserId);
        }
    }

    public class TweetsHandlers
        : IHandleMessages<DownloadNewTweetsFromTwitterApiRequest>
    {
        public Task Handle(DownloadNewTweetsFromTwitterApiRequest message, IMessageHandlerContext context)
        {
            var tweets = new List<DownloadNewTweetsFromTwitterApiResponse.Tweet>
            {
                new DownloadNewTweetsFromTwitterApiResponse.Tweet { AuthorId = "User_1", Text = "u1_t1" },
                new DownloadNewTweetsFromTwitterApiResponse.Tweet { AuthorId = "User_1", Text = "u1_t2" },
                new DownloadNewTweetsFromTwitterApiResponse.Tweet { AuthorId = "User_1", Text = "u1_t3" },
                new DownloadNewTweetsFromTwitterApiResponse.Tweet { AuthorId = "User_2", Text = "u2_t1" },
                new DownloadNewTweetsFromTwitterApiResponse.Tweet { AuthorId = "User_2", Text = "u2_t2" },
                new DownloadNewTweetsFromTwitterApiResponse.Tweet { AuthorId = "User_2", Text = "u2_t3" },
                new DownloadNewTweetsFromTwitterApiResponse.Tweet { AuthorId = "User_2", Text = "u2_t4" },
                new DownloadNewTweetsFromTwitterApiResponse.Tweet { AuthorId = "User_3", Text = "u3_t1" },
                new DownloadNewTweetsFromTwitterApiResponse.Tweet { AuthorId = "User_3", Text = "u3_t2" },
                new DownloadNewTweetsFromTwitterApiResponse.Tweet { AuthorId = "User_3", Text = "u3_t3" },
                new DownloadNewTweetsFromTwitterApiResponse.Tweet { AuthorId = "User_3", Text = "u3_t4" },
                new DownloadNewTweetsFromTwitterApiResponse.Tweet { AuthorId = "User_3", Text = "u3_t5" }
            };

            var result = tweets.Where(tweet => tweet.AuthorId == message.UserId).Select(tweet => tweet);
            return context.Reply(new DownloadNewTweetsFromTwitterApiResponse
            {
                Tweets = result.ToList(),
            });
        }
    }
}
