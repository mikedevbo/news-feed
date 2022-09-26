﻿using Microsoft.AspNetCore.Mvc;
using NewsFeed.Server.Models.Messaging.Commands;
using NewsFeed.Server.Models.Messaging.Messages;
using NewsFeed.Shared;
using NServiceBus;

[ApiController]
[Route("[controller]")]
public class NewsFeedController : ControllerBase
{
    private readonly IMessageSession messageSession;

    public NewsFeedController(IMessageSession messageSession)
    {
        this.messageSession = messageSession;
    }

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

    [HttpGet("GetDownloadedTweets")]
    public async Task<IEnumerable<TweetDto>> GetDownloadedTweets(string userId)
    {
        var result = new List<TweetDto>();
        var message = new GetDownloadedTweetsRequest { UserId = userId };
        var response = await messageSession.Request<GetDownloadedTweetsResponse>(message);

        if (response.Data is not null)
        {
            response.Data.Tweets.ForEach(t => result.Add(new TweetDto
            {
                UserId = response.Data.UserId,
                Text = t.Text
            }));
        }

        return result;
    }

    [HttpGet("DownloadNewTweets")]
    public async Task DownloadNewTweets(string userId)
    {
        var message = new DownloadNewTweets { UserId = userId };
        await messageSession.Send(message).ConfigureAwait(false);
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