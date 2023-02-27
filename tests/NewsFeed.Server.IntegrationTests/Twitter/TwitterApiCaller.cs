using NewsFeed.Shared.Twitter;
using System.Net.Http.Json;
using static NewsFeed.Shared.Twitter.StartDownloadingTweetsRequest;

namespace NewsFeed.Server.IntegrationTests.Twitter
{
    [TestFixture]
    [Explicit]
    public  class TwitterApiCaller
    {
        private static readonly HttpClient httpClient = new HttpClient();

        [SetUp]
        public void Setup()
        {
            httpClient.BaseAddress = new Uri(@"https://localhost:7057/");
        }

        [Test]
        [Explicit]
        public async Task StartDownloadingTweets_Call_Success()
        {
            // Arrange
            var users = new List<UserData> {  new UserData(1, "1"), new UserData(2, "2") };
            var command = new StartDownloadingTweetsRequest(users);

            // Act
            var result = await httpClient.PostAsJsonAsync(typeof(StartDownloadingTweetsRequest).Name, command);
            result.EnsureSuccessStatusCode();

            // Assert
            Assert.Pass();
        }
    }
}
