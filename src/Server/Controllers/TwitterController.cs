using Microsoft.AspNetCore.Mvc;
using NewsFeed.Server.Models.Twitter;
using NewsFeed.Server.Models.Twitter.Entity;
using NewsFeed.Shared.Twitter.Dto;
using NewsFeed.Shared.Twitter.Model;

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
    public async Task<MenuItems> GetMenu(int accountId)
    {
        return await this.twitterRepository.GetMenu(accountId);
    }

    [HttpGet("AddGroup")]
    public async Task<Group> AddGroup(int accountId, string groupName)
    {
        //TODO: Add validation
        return await this.twitterRepository.SaveGroup(new TwitterGroup() { AccountId = accountId, Name = groupName });
    }

    //[HttpGet("AddUser")]
    //public async Task<UserResponse> AddUser(string userName, int groupId)
    //{
    //    var user = await this.twitterApiClient.GetUser(userName);
    //    return await this.twitterRepository.SaveUser(userName, groupId, user.Id);
    //}
}