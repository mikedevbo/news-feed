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
                    ?? new TwitterMenuResponse();
        }

        public async Task<GroupResponse> AddGroup(int accountId, string groupName)
        {
            return await this.httpClient.GetFromJsonAsync<GroupResponse>($"Twitter/AddGroup?accountId={accountId}&groupName={groupName}")
                ?? new GroupResponse();
        }

        public async Task AddUser(string userName, int groupId)
        {
            await this.httpClient.GetAsync($"Twitter/AddUser?userName={userName}&groupId={groupId}");
        }
    }
}