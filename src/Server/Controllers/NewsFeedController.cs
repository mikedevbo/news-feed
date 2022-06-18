using Microsoft.AspNetCore.Mvc;
using NewsFeed.Shared;

[ApiController]
[Route("[controller]")]
public class NewsFeedController : ControllerBase
{
    [HttpGet("GetAccounts")]
    public IEnumerable<Account> GetAccounts()
    {
        return new List<Account>
        {
            new Account { Id = "User_1", GroupName = "General" },
            new Account { Id = "User_2", GroupName = "General" },
            new Account { Id = "User_3", GroupName = "General" },
            new Account { Id = "User_4", GroupName = "IT" },
            new Account { Id = "User_5", GroupName = "IT" },
            new Account { Id = "User_6", GroupName = "IT" },
            new Account { Id = "User_7", GroupName = "Sport" },
            new Account { Id = "User_8", GroupName = "Sport" },
            new Account { Id = "User_9", GroupName = "Sport" }
        };
    }
}
