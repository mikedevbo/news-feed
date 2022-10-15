using Microsoft.Extensions.Configuration;
using NewsFeed.Server.Twitter.Database;
using System.Data.Common;
using System.Data.SqlClient;

namespace NewsFeed.Server.IntegrationTests
{
    [TestFixture]
    [Explicit]
    public class TwitterRepositoryTests
    {
        private DbConnection connection;
        private DbTransaction transaction;
        private TwitterRepository twitterRepository;

        [SetUp]
        public async Task SetUp()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.development.json", false, true)
                .Build();

            this.connection = new SqlConnection(config.GetValue<string>(Constants.ConnectionStringPersistenceKey));
            await this.connection.OpenAsync();
            this.transaction = await this.connection.BeginTransactionAsync();
            this.twitterRepository = new TwitterRepository(this.connection, this.transaction);
        }

        [TearDown]
        public void TearDown()
        {
            this.transaction.Commit();
            this.transaction.Dispose();
            this.connection.Close();
            this.connection.Dispose();
        }

        [Test]
        [Explicit]
        public async Task SetTweetsDownloadingState_Execute_ProperResult()
        {
            // Arrange
            const int userId = 1;
            const bool isTweetsDownloading = false;

            // Act
            await this.twitterRepository.SetTweetsDownloadingState(userId, isTweetsDownloading);

            // Assert
            Assert.Pass();
        }

        [Test]
        [Explicit]
        public async Task ClearOldTweets_Execute_ProperResult()
        {
            // Arrange
            const int userId = 1;
            var createdAt = Convert.ToDateTime("2022-10-02 08:00:00.000");

            // Act
            await this.twitterRepository.ClearOldTweets(userId, createdAt);

            // Assert
            Assert.Pass();
        }
    }
}
