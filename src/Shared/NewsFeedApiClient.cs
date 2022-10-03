using NewsFeed.Shared.Dto;
using System.Net.Http.Json;

namespace NewsFeed.Shared
{
    public class NewsFeedApiClient : INewsFeedApiClient
    {
        private readonly HttpClient httpClient;

        public NewsFeedApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<TwitterMenuResponse> GetTwitterMenu(int accountId)
        {
            return
                await this.httpClient.GetFromJsonAsync<TwitterMenuResponse>($"Users/GetTwitterMenu?accountId={accountId}")
                ?? new TwitterMenuResponse(new List<TwitterMenuResponse.Group>());
        }

        public async Task AddTwitterGroup(int accountId, string groupName)
        {
            await this.httpClient.GetAsync($"Users/AddTwitterGroup?accountId={accountId}&groupName={groupName}");
        }

        public async Task AddUser(string userName, int groupId)
        {
            await this.httpClient.GetAsync($"Users/AddUser?userName={userName}&groupId={groupId}");
        }
    }
}