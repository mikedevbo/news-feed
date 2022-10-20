using Microsoft.Extensions.Configuration;
using NewsFeed.Server.Twitter.Messaging.Commands;
using NServiceBus;
using System.Reflection;

namespace NewsFeed.Server.IntegrationTests.Twitter
{
    [TestFixture]
    [Explicit]
    public class TwitterMessageSender
    {
        IEndpointInstance endpointInstance;

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
                 (typeof(DownloadTweetsRequests).Assembly, typeof(NewsFeedController).Assembly.GetName().Name!)
             });
            endpointInstance = await Endpoint.Start(endpointConfig);
        }

        [TearDown]
        public async Task TearDown()
        {
            await endpointInstance.Stop();
        }

        [Test]
        [Explicit]
        public async Task DownloadTweets_Send_Success()
        {
            // Arrange
            const int userId = 1;
            const string twitterUserId = "200";

            var message = new DownloadTweets(userId, twitterUserId);

            // Act
            await endpointInstance.Send(message);

            // Assert
            Assert.Pass();
        }
    }
}
