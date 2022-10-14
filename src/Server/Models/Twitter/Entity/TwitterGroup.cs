namespace NewsFeed.Server.Models.Twitter.Entity
{
    public record TwitterGroup(int Id, string Name, int AccountId)
    {
        public TwitterGroup() : this(default, string.Empty, default) { }

        public TwitterGroup(string Name, int AccountId) : this(default, Name, AccountId) { }
    }
}
