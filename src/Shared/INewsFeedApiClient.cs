namespace NewsFeed.Shared
{
    public interface INewsFeedApiClient
    {
        Task AddUser(string userName, int groupId);
    }
}
