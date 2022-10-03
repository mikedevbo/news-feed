using NewsFeed.Shared.Dto;
using System.Net.Http.Json;

namespace NewsFeed.Shared
{
    public class NewsFeedTwitterApiClient : INewsFeedTwitterApiClient
    {
        private readonly HttpClient httpClient;

        public NewsFeedTwitterApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<TwitterMenuResponse> GetMenu(int accountId)
        {
            return
                await this.httpClient.GetFromJsonAsync<TwitterMenuResponse>($"Twitter/GetMenu?accountId={accountId}")
                ?? new TwitterMenuResponse(new List<TwitterMenuResponse.Group>());
        }

        public async Task AddGroup(int accountId, string groupName)
        {
            await this.httpClient.GetAsync($"Twitter/AddGroup?accountId={accountId}&groupName={groupName}");
        }

        public async Task AddUser(string userName, int groupId)
        {
            await this.httpClient.GetAsync($"Twitter/AddUser?userName={userName}&groupId={groupId}");
        }
    }
}