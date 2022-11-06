using NewsFeed.Shared.Twitter;
using NewsFeed.Shared.Twitter.Commands;
using System.Net.Http.Json;
using static NewsFeed.Shared.Twitter.Commands.StartDownloadingTweets;

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
            var command = new StartDownloadingTweets(users);

            // Act
            await httpClient.PostAsJsonAsync($"/twitter/tweets/startdownoading", command);

            // Assert
            Assert.Pass();
        }
    }
}
