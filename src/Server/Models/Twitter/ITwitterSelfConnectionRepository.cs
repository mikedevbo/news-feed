namespace NewsFeed.Server.Models.Twitter
{
    public interface ITwitterSelfConnectionRepository
    {
        Task SaveUser(string userName, int groupId, string twitterUserId);
    }
}
