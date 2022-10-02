using Microsoft.Extensions.Configuration;
using NewsFeed.Server.Models.Twitter;

namespace NewsFeed.Server.IntegrationTests
{
    [TestFixture]
    [Explicit]
    public class TwitterSelfConnectionRepositoryTests
    {
        private TwitterSelfConnectionRepository repository;

        [SetUp]
        public void SetUp()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.development.json", false, true)
                .Build();

            this.repository = new TwitterSelfConnectionRepository(config);
        }

        [Test]
        [Explicit]
        public async Task SaveUser_Execute_ProperResult()
        {
            // Arrange
            const string userName = "test_user2";
            const int groupId = 1;
            const string twitterUserId = "12345";

            // Act
            await this.repository.SaveUser(userName, groupId, twitterUserId);

            // Assert
            Assert.Pass();
        }
    }
}
