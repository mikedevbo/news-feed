using System.Text.Json;

namespace NewsFeed.Shared.IntegrationTests
{
    [TestFixture]
    [Explicit]
    public class NewsFeedApiClientTests
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private NewsFeedApiClient apiClient;

        [SetUp]
        public void Setup()
        {
            httpClient.BaseAddress = new Uri(@"https://localhost:7057/");
            this.apiClient = new NewsFeedApiClient(httpClient);
        }

        [Test]
        [Explicit]
        public async Task GetTwitterMenu_Execute_ProperResult()
        {
            // Arrange
            const int accountId = 1;

            // Act
            var result = await this.apiClient.GetTwitterMenu(accountId);

            // Assert
            Console.WriteLine(JsonSerializer.Serialize(result));
            Assert.Pass();
        }

        [Test]
        [Explicit]
        public async Task AddTwitterGroup_Execute_ProperResult()
        {
            // Arrange
            const int accountId = 1;
            const string groupName = "group_from_integration_test";

            // Act
            await this.apiClient.AddTwitterGroup(accountId, groupName);

            // Assert
            Assert.Pass();
        }

        [Test]
        [Explicit]
        public async Task AddUser_Execute_ProperResult()
        {
            // Arrange
            const string userName = "user_from_integration_test";
            const int groupId = 1;

            // Act
            await this.apiClient.AddUser(userName, groupId);

            // Assert
            Assert.Pass();
        }
    }
}