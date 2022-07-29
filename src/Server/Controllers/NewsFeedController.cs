using Microsoft.AspNetCore.Mvc;
using NewsFeed.Shared;

[ApiController]
[Route("[controller]")]
public class NewsFeedController : ControllerBase
{
    private List<Account> accounts = new List<Account>
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

    private List<Tweet> tweets = new List<Tweet>
    {
        new Tweet { AccountId = "User_1", Text = "u1_t1" },
        new Tweet { AccountId = "User_1", Text = "u1_t2" },
        new Tweet { AccountId = "User_1", Text = "u1_t3" },
        new Tweet { AccountId = "User_2", Text = "u2_t1" },
        new Tweet { AccountId = "User_2", Text = "u2_t2" },
        new Tweet { AccountId = "User_2", Text = "u2_t3" },
        new Tweet { AccountId = "User_2", Text = "u2_t4" },
        new Tweet { AccountId = "User_3", Text = "u3_t1" },
        new Tweet { AccountId = "User_3", Text = "u3_t2" },
        new Tweet { AccountId = "User_3", Text = "u3_t3" },
        new Tweet { AccountId = "User_3", Text = "u3_t4" },
        new Tweet { AccountId = "User_3", Text = "u3_t5" }
    };

    private Dictionary<string, int> attemptSimulator = new Dictionary<string, int>()
    {
        { "User_1", 0 },
        { "User_2", 0 },
        { "User_3", 0 },
    };

    [HttpGet("GetAccounts")]
    public IEnumerable<Account> GetAccounts()
    {
        return accounts;
    }

    [HttpGet("GetTweets")]
    public IEnumerable<Tweet> GetTweets(string accountId)
    {
        return this.tweets.Where(tweet => tweet.AccountId == accountId).Select(tweet => tweet);
    }

    [HttpPost("StartGettingNewTweets")]
    public void StartGettingNewTweets(string accountId)
    {
        var account = this.accounts.Find(acc => acc.Id == accountId);
        if (account != null)
        {
            account.IsGettingsNewTweets = true;
        }
    }

    [HttpPost("IsGettingNewTweetsReady")]
    public bool IsGettingNewTweetsReady(string accountId)
    {
        if (this.attemptSimulator.ContainsKey(accountId))
        {
            if (this.attemptSimulator[accountId] < 3)
            {
                this.attemptSimulator[accountId] += 1;
                return false;
            }

            this.attemptSimulator[accountId] = 0;

            var account = this.accounts.Find(acc => acc.Id == accountId);
            if (account != null)
            {
                account.IsGettingsNewTweets = false;
            }

            return true;
        }

        return true;
    }
}
