namespace NewsFeed.Shared.Dto
{
    public record TwitterMenuResponse(List<TwitterMenuResponse.Group> Groups)
    {
        public record Group(
            int Id,
            string Name,
            List<User> Users);

        public record User(
            int Id,
            string Name,
            string TwitterUserId,
            bool IsTweetsDownloading,
            bool IsSelected);
    }
}
