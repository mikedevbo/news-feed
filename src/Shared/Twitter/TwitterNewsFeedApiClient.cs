﻿using NewsFeed.Shared.Twitter.Dto;
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

        public async Task<List<Tweet>> GetTweets(int userId)
        {
            return await httpClient.GetFromJsonAsync<List<Tweet>>($"Twitter/GetTweets?userId={userId}")
            ?? new List<Tweet>();
        }
    }
}