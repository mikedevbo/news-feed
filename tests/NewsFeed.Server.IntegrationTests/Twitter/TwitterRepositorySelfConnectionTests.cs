using Microsoft.Extensions.Configuration;
using NewsFeed.Server.Models.Twitter.Entity;
using NewsFeed.Server.Twitter.Database;
using System.Text.Json;

namespace NewsFeed.Server.IntegrationTests.Twitter
{
    [TestFixture]
    [Explicit]
    public class TwitterRepositorySelfConnectionTests
    {
        private TwitterRepositorySelfConnection twitterRepository;

        [SetUp]
        public void SetUp()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.development.json", false, true)
                .Build();

            twitterRepository = new TwitterRepositorySelfConnection(config);
        }

        [Test]
        [Explicit]
        public async Task GetMenu_Execute_ProperResult()
        {
            // Arrange
            const int accountId = 1;

            // Act
            var result = await twitterRepository.GetMenu(accountId);

            // Assert
            Console.WriteLine(JsonSerializer.Serialize(result.Groups));
            Console.WriteLine(JsonSerializer.Serialize(result.Users));
            Console.WriteLine(JsonSerializer.Serialize(result.GroupUsers));
            Assert.Pass();
        }

        [Test]
        [Explicit]
        public async Task SaveGroup_Execute_ProperResult()
        {
            // Arrange
            var group = new TwitterGroup("test_group_2", 1);

            // Act
            var result = await twitterRepository.SaveGroup(group);

            // Assert
            Console.WriteLine(JsonSerializer.Serialize(result));
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Explicit]
        public async Task SaveUser_Execute_ProperResult()
        {
            // Arrange
            var user = new TwitterUser("test_user2", 1);

            var userApi = new TwitterUsersApi("12345");

            // Act
            var result = await twitterRepository.SaveUser(user, userApi);

            // Assert
            Console.WriteLine(JsonSerializer.Serialize(result));
            Assert.That(result, Is.Not.Null);
        }
    }
}
