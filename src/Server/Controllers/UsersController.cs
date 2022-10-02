using Microsoft.AspNetCore.Mvc;
using NewsFeed.Server.Models.Twitter;

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

    [HttpGet("AddUser")]
    public async Task AddUser(string userName, int groupId)
    {
        var user = await this.twitterApiClient.GetUser(userName);
        await this.twitterRepository.SaveUser(userName, groupId, user.Id);
    }
}
