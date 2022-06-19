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

    [HttpGet("GetTweets")]
    public IEnumerable<Tweet> GetTweets(string accountId)
    {
        var result = new List<Tweet>();

        switch (accountId)
        {
            case "User_1":
                result.AddRange(new List<Tweet>
                {
                    new Tweet { AccountId = accountId, Text = "u1_t1" },
                    new Tweet { AccountId = accountId, Text = "u1_t2" },
                    new Tweet { AccountId = accountId, Text = "u1_t3" }
                });
                break;
            case "User_2":
                result.AddRange(new List<Tweet>
                {
                    new Tweet { AccountId = accountId, Text = "u2_t1" },
                    new Tweet { AccountId = accountId, Text = "u2_t2" },
                    new Tweet { AccountId = accountId, Text = "u2_t3" },
                    new Tweet { AccountId = accountId, Text = "u2_t4" }
                });
                break;
            case "User_3":
                result.AddRange(new List<Tweet>
                {
                    new Tweet { AccountId = accountId, Text = "u3_t1" },
                    new Tweet { AccountId = accountId, Text = "u3_t2" },
                    new Tweet { AccountId = accountId, Text = "u3_t3" },
                    new Tweet { AccountId = accountId, Text = "u3_t4" },
                    new Tweet { AccountId = accountId, Text = "u3_t5" }
                });
                break;
            default:
                break;
        }

        return result;
    }
}
