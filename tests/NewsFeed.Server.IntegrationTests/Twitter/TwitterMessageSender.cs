using Microsoft.Extensions.Configuration;
using NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga.Commands;
using NewsFeed.Shared.Twitter.Commands;
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
                 (typeof(StartDownloadingTweets).Assembly, typeof(StartDownloadingTweets).Assembly.GetName().Name!)
             });

            endpointInstance = await Endpoint.Start(endpointConfig);
        }

        [TearDown]
        public async Task TearDown()
        {
            await endpointInstance.Stop();
        }
    }
}
