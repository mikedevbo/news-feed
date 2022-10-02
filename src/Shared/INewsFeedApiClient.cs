using NewsFeed.Shared.Dto;

namespace NewsFeed.Shared
{
    public interface INewsFeedApiClient
    {
        Task<TwitterMenuResponse> GetTwitterMenu();

        Task AddUser(string userName, int groupId);
    }
}
