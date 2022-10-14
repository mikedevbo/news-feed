using Microsoft.Extensions.Configuration;
using NewsFeed.Server.Models.Twitter;
using NewsFeed.Server.Models.Twitter.Tables;
using System.Text.Json;

namespace NewsFeed.Server.IntegrationTests
{
    [TestFixture]
    [Explicit]
    public class TwitterSelfConnectionRepositoryTests
    {
        private TwitterSelfConnectionRepository twitterRepository;

        [SetUp]
        public void SetUp()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.development.json", false, true)
                .Build();

            this.twitterRepository = new TwitterSelfConnectionRepository(config);
        }

        [Test]
        [Explicit]
        public async Task GetMenu_Execute_ProperResult()
        {
            // Arrange
            const int accountId = 1;

            // Act
            var result = await this.twitterRepository.GetMenu(accountId);

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
            var group = new TwitterGroup()
            {
                Name = "test_group_2",
                AccountId = 1
            };

            // Act
            var result = await this.twitterRepository.SaveGroup(group);

            // Assert
            Console.WriteLine(JsonSerializer.Serialize(result));
            Assert.That(result, Is.Not.Null);
        }

        //[Test]
        //[Explicit]
        //public async Task SaveUser_Execute_ProperResult()
        //{
        //    // Arrange
        //    const string userName = "test_user2";
        //    const int groupId = 1;
        //    const string twitterUserId = "12345";

        //    // Act
        //    var result = await this.twitterRepository.SaveUser(userName, groupId, twitterUserId);

        //    // Assert
        //    Assert.That(result, Is.Not.Null);
        //}
    }
}
