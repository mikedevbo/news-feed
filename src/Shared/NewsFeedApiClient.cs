namespace NewsFeed.Shared
{
    public class NewsFeedApiClient : INewsFeedApiClient
    {
        private readonly HttpClient httpClient;

        public NewsFeedApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task AddUser(string userName, int groupId)
        {
            await this.httpClient.GetAsync($"Users/AddUser?userName={userName}&groupId={groupId}");
        }
    }
}