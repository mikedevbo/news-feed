namespace NewsFeed.Server.Models.Twitter
{
    public interface ITwitterSelfConnectionRepository
    {
        Task SaveUser(string userName, int groupId, string twitterUserId);

        Task SaveGroup(int accountId, string groupName);
    }
}
