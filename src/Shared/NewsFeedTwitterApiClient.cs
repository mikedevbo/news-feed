﻿using NewsFeed.Shared.Dto;
using NewsFeed.Shared.Twitter.Dto;
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

        public async Task<MenuItems> GetMenu(int accountId)
        {
            return
                await this.httpClient.GetFromJsonAsync<MenuItems>($"Twitter/GetMenu?accountId={accountId}")
                    ?? new MenuItems();
        }

        //public async Task<GroupResponse> AddGroup(int accountId, string groupName)
        //{
        //    return await this.httpClient.GetFromJsonAsync<GroupResponse>($"Twitter/AddGroup?accountId={accountId}&groupName={groupName}")
        //        ?? new GroupResponse();
        //}

        //public async Task<UserResponse> AddUser(string userName, int groupId)
        //{
        //    return await this.httpClient.GetFromJsonAsync<UserResponse>($"Twitter/AddUser?userName={userName}&groupId={groupId}")
        //        ?? new UserResponse();
        //}
    }
}