using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga;
using NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga.Commands;
using NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga.Messages;
using NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga.SagaData;
using NServiceBus.Testing;

namespace NewsFeed.Shared.Tests
{
    public class Tests
    {
        [Test]
        public async Task StartDownloadingTweetsForUser_NewUser_InitializeDownladAndClearOld()
        {
            // Arrange
            const int userId = 1;
            const string twitterUserId = "1";
            var saga = new TestableSaga<DownloadTweetsSaga, DownloadTweetsSagaData>();
            
            var startDownloadingTweetsForUser = new StartDownloadingTweetsForUser(userId, twitterUserId);
            
            // Act - handle new user
            var result = await saga.Handle(startDownloadingTweetsForUser);
            
            // Assert
            var sentDownloadTweets = result.FindSentMessage<DownloadTweets>();
            var requestedClearOldTweetsTimeout = result.FindTimeoutMessage<ClearOldTweetsTimeout>();
            
            Assert.That(sentDownloadTweets, Is.Not.Null);
            Assert.That(sentDownloadTweets.UserId, Is.EqualTo(userId));
            Assert.That(sentDownloadTweets.TwitterUserId, Is.EqualTo(twitterUserId));
            Assert.That(result.Completed, Is.False);

            Assert.That(requestedClearOldTweetsTimeout, Is.Not.Null);

            // Act - handle existing user
            result = await saga.Handle(startDownloadingTweetsForUser);

            // Assert
            sentDownloadTweets = result.FindSentMessage<DownloadTweets>();
            requestedClearOldTweetsTimeout = result.FindTimeoutMessage<ClearOldTweetsTimeout>();

            Assert.That(sentDownloadTweets, Is.Not.Null);
            Assert.That(sentDownloadTweets.UserId, Is.EqualTo(userId));
            Assert.That(sentDownloadTweets.TwitterUserId, Is.EqualTo(twitterUserId));
            Assert.That(result.Completed, Is.False);

            Assert.That(requestedClearOldTweetsTimeout, Is.Null);

            // Act - times up
            var timeoutResult = await saga.AdvanceTime(TimeSpan.FromDays(8));

            // Arrange
            var sentClearOldTweets = timeoutResult.First().FindSentMessage<ClearOldTweets>();
            requestedClearOldTweetsTimeout = timeoutResult.First().FindTimeoutMessage<ClearOldTweetsTimeout>();

            Assert.That(sentClearOldTweets, Is.Not.Null);
            Assert.That(sentClearOldTweets.UserId, Is.EqualTo(userId));
            Assert.That(timeoutResult.Length, Is.EqualTo(1));
            Assert.That(result.Completed, Is.False);

            Assert.That(requestedClearOldTweetsTimeout, Is.Not.Null);
        }
    }
}