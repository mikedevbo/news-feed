using NewsFeed.Server.Twitter.Messaging.ClearTweetsPolicy.Commands;
using NewsFeed.Server.Twitter.Messaging.ClearTweetsPolicy.Messages;
using NewsFeed.Server.Twitter.Messaging.ClearTweetsPolicy.Saga;
using NewsFeed.Server.Twitter.Messaging.ClearTweetsPolicy.SagaData;
using NServiceBus.Testing;

namespace NewsFeed.Shared.Tests
{
    public class Tests
    {
        [Test]
        public async Task ClearTweetsSaga_SimulateFlow_TestPassed()
        {
            // Arrange
            const int instanceId = 1;
            var saga = new TestableSaga<ClearTweetsSaga, ClearTweetsSagaData>();
            
            var initializeClearingTweets = new InitializeClearingTweets(instanceId);
            
            // Act
            var result = await saga.Handle(initializeClearingTweets);
            
            // Assert
            var clearTweetsTimeout = result.FindTimeoutMessage<ClearTweetsTimeout>();
            
            Assert.Multiple(() =>
            {
                Assert.That(clearTweetsTimeout, Is.Not.Null);
                Assert.That(result.SagaDataSnapshot.InstanceId, Is.EqualTo(instanceId));
                Assert.That(result.Completed, Is.False);
            });

            // Act - times up
            var timeoutResult = await saga.AdvanceTime(TimeSpan.FromDays(8));

            // Assert
            var sentClearTweets = timeoutResult.First().FindSentMessage<ClearTweets>();
            clearTweetsTimeout = result.FindTimeoutMessage<ClearTweetsTimeout>();
            
            Assert.Multiple(() =>
            {
                Assert.That(sentClearTweets, Is.Not.Null);
                Assert.That(clearTweetsTimeout, Is.Not.Null);
                Assert.That(result.Completed, Is.False);
            });
        }
    }
}