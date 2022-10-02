using Microsoft.AspNetCore.Mvc;
using NewsFeed.Server.Models.Twitter;
using NewsFeed.Shared.Dto;

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
    public Task<TwitterMenuResponse> GetTwitterMenu()
    {
        return Task.FromResult(new TwitterMenuResponse(new List<TwitterMenuResponse.Group>
        {
            new TwitterMenuResponse.Group(100, "test_group", new List<TwitterMenuResponse.User>
            {
                new TwitterMenuResponse.User(100, "test_user", "200", false, false),
                new TwitterMenuResponse.User(101, "test_user_2", "201", false, false)
            }),
            new TwitterMenuResponse.Group(101, "test_group_2", new List<TwitterMenuResponse.User>
            {
                new TwitterMenuResponse.User(102, "test_user_3", "202", false, false),
                new TwitterMenuResponse.User(103, "test_user_4", "203", false, false)
            })
        }));
    }

    [HttpGet("AddUser")]
    public async Task AddUser(string userName, int groupId)
    {
        var user = await this.twitterApiClient.GetUser(userName);
        await this.twitterRepository.SaveUser(userName, groupId, user.Id);
    }
}
