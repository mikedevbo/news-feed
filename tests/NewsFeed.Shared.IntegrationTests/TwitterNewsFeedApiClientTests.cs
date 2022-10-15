using System.Text.Json;
using NewsFeed.Shared.Twitter;

namespace NewsFeed.Shared.IntegrationTests
{
    [TestFixture]
    [Explicit]
    public class TwitterNewsFeedApiClientTests
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private TwitterNewsFeedApiClient apiClient;

        [SetUp]
        public void Setup()
        {
            httpClient.BaseAddress = new Uri(@"https://localhost:7057/");
            this.apiClient = new TwitterNewsFeedApiClient(httpClient);
        }

        [Test]
        [Explicit]
        public async Task GetMenu_Execute_ProperResult()
        {
            // Arrange
            const int accountId = 1;

            // Act
            var result = await this.apiClient.GetMenu(accountId);

            // Assert
            Console.WriteLine(JsonSerializer.Serialize(result.Groups));
            Console.WriteLine(JsonSerializer.Serialize(result.Users));
            Console.WriteLine(JsonSerializer.Serialize(result.GroupUsers));
            Assert.Pass();
        }

        [Test]
        [Explicit]
        public async Task AddGroup_Execute_ProperResult()
        {
            // Arrange
            const int accountId = 1;
            const string groupName = "group_from_integration_test";

            // Act
            var result = await this.apiClient.AddGroup(groupName, accountId);

            // Assert
            Console.WriteLine(JsonSerializer.Serialize(result));
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Explicit]
        public async Task AddUser_Execute_ProperResult()
        {
            // Arrange
            const string userName = "user_from_integration_test";
            const int groupId = 1;

            // Act
            var result = await this.apiClient.AddUser(userName, groupId);

            // Assert
            Console.WriteLine(JsonSerializer.Serialize(result));
            Assert.That(result, Is.Not.Null);
        }
    }
}