using Microsoft.AspNetCore.Mvc;
using NewsFeed.Server.Models.Twitter.Entity;
using NewsFeed.Server.Twitter.Database;
using NewsFeed.Server.Twitter.ExternalApi;
using NewsFeed.Shared.Twitter.Dto;
using NewsFeed.Shared.Twitter.Model;
using System.Text.Json;

[ApiController]
[Route("[controller]")]
public class TwitterController : ControllerBase
{
    private readonly ITwitterApiClient twitterApiClient;
    private readonly ITwitterRepositorySelfConnection twitterRepository;

    public TwitterController(
        ITwitterApiClient twitterApiClient,
        ITwitterRepositorySelfConnection twitterRepository)
    {
        this.twitterApiClient = twitterApiClient;
        this.twitterRepository = twitterRepository;
    }

    [HttpGet("AddGroup")]
    public async Task<Group> AddGroup(string groupName, int accountId)
    {
        //TODO: add validation
        return await this.twitterRepository.SaveGroup(new TwitterGroup(groupName, accountId));
    }

    [HttpGet("AddUser")]
    public async Task<User> AddUser(string userName, int groupId)
    {
        //TODO: add validation
        var userApi = await this.twitterApiClient.GetUser(userName);

        return await this.twitterRepository.SaveUser(
            new TwitterUser(userName, groupId),
            new TwitterUsersApi(userApi.Id));
    }

    [HttpGet("Handle")]
    public async Task<List<Tweet>> Handle(int userId)
    {
        return await this.twitterRepository.GetTweets(userId);
    }
}