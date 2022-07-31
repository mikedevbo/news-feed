using NewsFeed.Server.Models;
using System.Reflection;

namespace NewsFeed.Server.IntegrationTests
{
    public class TwitterRepositoryTests
    {
        private string basePath;
        private ITwitterRepository twitterRepository;
        
        [SetUp]
        public void Setup()
        {
            this.basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            this.twitterRepository = new TwitterRepository(this.basePath);
        }

        [Test]
        [Explicit]
        public void GivenGetDownloadedTweets_WhenNoTweets_ThenEmptytList()
        {
            // Arrange
            const string accountId = "123";

            // Act
            var tweets = this.twitterRepository.GetDownloadedTweets(accountId);

            // Assert
            Assert.That(tweets, Is.Empty);
        }

        [Test]
        [Explicit]
        public void tGivenGetDownloadedTweets_WhenFileNotExists_ThenEmptyTweetList()
        {
            // Arrange
            const string accountId = "123";
            string filePath = $"{this.basePath}{Path.DirectorySeparatorChar}{accountId}.json";
            using (File.Create(filePath));

            // Act
            var tweets = this.twitterRepository.GetDownloadedTweets(accountId);

            // Assert
            Assert.That(tweets, Is.Empty);
        }
    }
}