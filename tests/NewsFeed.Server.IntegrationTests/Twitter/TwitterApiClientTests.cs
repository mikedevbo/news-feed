using Microsoft.Extensions.Configuration;
using NewsFeed.Server.Twitter.ExternalApi;
using System.Text.Json;

namespace NewsFeed.Server.IntegrationTests.Twitter
{
    [TestFixture]
    [Explicit]
    public class TwitterApiClientTests
    {
        private TwitterApiClient? twitterApiClient;

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.development.json", false, true)
                .Build();

            twitterApiClient = new TwitterApiClient(config.GetValue<string>("TwitterToken"));
        }

        [Test]
        [Explicit]
        public async Task GetUser_SampleUser_UserData()
        {
            // Arrange
            const string userName = "particularsw";

            // Act
            var result = await twitterApiClient!.GetUser(userName);

            // Assert
            Assert.That(result, Is.Not.Null);
            Console.WriteLine(JsonSerializer.Serialize(result));
        }

        [Test]
        [Explicit]
        public async Task GetTweets_SampleUserId_Tweets()
        {
            // Arrange
            const string userId = "some_Id";

            // Act
            var result = await twitterApiClient!.GetTweets(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Console.WriteLine(JsonSerializer.Serialize(result));
        }
    }
}