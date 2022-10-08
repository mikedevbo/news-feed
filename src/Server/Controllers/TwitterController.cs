using Microsoft.AspNetCore.Mvc;
using NewsFeed.Server.Models.Twitter;
using NewsFeed.Shared.Dto;
using System.Reflection.Metadata.Ecma335;

[ApiController]
[Route("[controller]")]
public class TwitterController : ControllerBase
{
    private readonly ITwitterApiClient twitterApiClient;
    private readonly ITwitterSelfConnectionRepository twitterRepository;

    public TwitterController(
        ITwitterApiClient twitterApiClient,
        ITwitterSelfConnectionRepository twitterRepository)
    {
        this.twitterApiClient = twitterApiClient;
        this.twitterRepository = twitterRepository;
    }

    [HttpGet("GetMenu")]
    public async Task<TwitterMenuResponse> GetMenu(int accountId)
    {
        return await this.twitterRepository.GetMenu(accountId);
    }

    [HttpGet("AddGroup")]
    public async Task<GroupResponse> AddGroup(int accountId, string groupName)
    {
        return await this.twitterRepository.SaveGroup(accountId, groupName);
    }

    [HttpGet("AddUser")]
    public async Task<UserResponse> AddUser(string userName, int groupId)
    {
        var user = await this.twitterApiClient.GetUser(userName);
        return await this.twitterRepository.SaveUser(userName, groupId, user.Id);
    }
}