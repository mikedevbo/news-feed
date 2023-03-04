using Microsoft.Extensions.Configuration;
using NewsFeed.Server.Twitter.Messaging.ClearTweetsPolicy.Commands;
using NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga.Commands;
using NServiceBus;
using System.Reflection;

namespace NewsFeed.Server.IntegrationTests.Twitter
{
    [TestFixture]
    [Explicit]
    public class TwitterMessageSender
    {
        private readonly string endpointName = typeof(EndpointCommonConfig).Assembly.GetName().Name!;
        private IEndpointInstance endpointInstance;

        [SetUp]
        public async Task SetUp()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.development.json", false, true)
                .Build();

            var endpointConfig = EndpointCommonConfig.Get(
                "MessageSender.Test",
                config,
                new List<(Assembly, string)>
             {
                 (typeof(StartDownloadingTweets).Assembly, endpointName),
                 (typeof(StartDownloadingTweetsForUser).Assembly, endpointName),
                 (typeof(InitializeClearingTweets).Assembly, endpointName)
             });

            endpointInstance = await Endpoint.Start(endpointConfig);
        }


        [Test]
        [Explicit]
        public async Task InitializeClearingTweets_Send_Success()
        {
            // Arrange
            var message = new InitializeClearingTweets(1);

            // Act
            await this.endpointInstance.Send(message);

            // Assert
            Assert.Pass();
        }

        [Test]
        [Explicit]
        public async Task StartDownloadingTweets_Send_Success()
        {
            // Arrange
            var message = new StartDownloadingTweets(new List<(int, string)> { (1, "1"), (2, "2") });

            // Act
            await this.endpointInstance.Send(message);

            // Assert
            Assert.Pass();
        }

        [Test]
        [Explicit]
        public async Task StartDownloadingTweetsForUser_Send_Success()
        {
            // Arrange
            var message = new StartDownloadingTweetsForUser(1, "1");

            // Act
            await this.endpointInstance.Send(message);

            // Assert
            Assert.Pass();
        }

        [Test]
        [Explicit]
        public async Task DownloadTweets_Send_Success()
        {
            // Arrange
            var message = new DownloadTweets(1, "1");

            // Act
            await this.endpointInstance.Send(message);

            // Assert
            Assert.Pass();
        }

        [TearDown]
        public async Task TearDown()
        {
            await endpointInstance.Stop();
        }
    }
}
