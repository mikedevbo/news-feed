using NewsFeed.Shared.Twitter.Dto;
using NewsFeed.Shared.Twitter.Model;
using System.Net.Http.Json;

namespace NewsFeed.Shared.Twitter
{
    public class TwitterNewsFeedApiClient : ITwitterNewsFeedApiClient
    {
        private readonly HttpClient httpClient;

        public TwitterNewsFeedApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<MenuItems> GetMenu(int accountId)
        {
            return
                await httpClient.GetFromJsonAsync<MenuItems>($"Twitter/GetMenu?accountId={accountId}")
                    ?? new MenuItems();
        }

        public async Task<Group> AddGroup(string groupName, int accountId)
        {
            return await httpClient.GetFromJsonAsync<Group>($"Twitter/AddGroup?groupName={groupName}&accountId={accountId}")
                ?? new Group();
        }

        public async Task<User> AddUser(string userName, int groupId)
        {
            return await httpClient.GetFromJsonAsync<User>($"Twitter/AddUser?userName={userName}&groupId={groupId}")
                ?? new User();
        }
    }
}