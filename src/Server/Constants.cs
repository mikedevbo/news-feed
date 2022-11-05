namespace NewsFeed.Server
{
    public static class Constants
    {
        public static string ConnectionStringPersistenceKey = @"ConnectionStrings:NsbPersistence";

        public static string ConnectionStringTransportKey = @"ConnectionStrings:NsbTransport";

        public static IConfiguration? Configuration;
        public static void Initialize(IConfiguration config)
        {
            Configuration = config;
        }
    }
}
