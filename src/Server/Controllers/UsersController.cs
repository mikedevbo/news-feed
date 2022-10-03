using Microsoft.AspNetCore.Mvc;
using NewsFeed.Server.Models.Twitter;
using NewsFeed.Shared.Dto;
using System.Reflection.Metadata.Ecma335;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ITwitterApiClient twitterApiClient;
    private readonly ITwitterSelfConnectionRepository twitterRepository;

    public UsersController(
        ITwitterApiClient twitterApiClient,
        ITwitterSelfConnectionRepository twitterRepository)
    {
        this.twitterApiClient = twitterApiClient;
        this.twitterRepository = twitterRepository;
    }

    [HttpGet("GetTwitterMenu")]
    public async Task<TwitterMenuResponse> GetTwitterMenu(int accountId)
    {
        return await this.twitterRepository.GetTwitterMenu(accountId);
    }

    [HttpGet("AddTwitterGroup")]
    public async Task AddTwitterGroup(int accountId, string groupName)
    {
        await this.twitterRepository.SaveGroup(accountId, groupName);
    }

    [HttpGet("AddUser")]
    public async Task AddUser(string userName, int groupId)
    {
        var user = await this.twitterApiClient.GetUser(userName);
        await this.twitterRepository.SaveUser(userName, groupId, user.Id);
    }
}