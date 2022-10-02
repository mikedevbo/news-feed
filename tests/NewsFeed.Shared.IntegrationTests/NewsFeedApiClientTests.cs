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
        public async Task GetTwitterMenu_Execute_ProperResult()
        {
            // Arrange

            // Act
            var result = await this.apiClient.GetTwitterMenu();

            // Assert
            Console.WriteLine(JsonSerializer.Serialize(result));
            Assert.Pass();
        }

        [Test]
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