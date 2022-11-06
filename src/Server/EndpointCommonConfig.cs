using NServiceBus.Logging;
using NServiceBus;
using System.Reflection;
using System.Data.SqlClient;

namespace NewsFeed.Server
{
    public static class EndpointCommonConfig
    {
        public static EndpointConfiguration Get(
            string endpointName,
            IConfiguration configuration,
            List<(Assembly assembly, string endpointName)> routingDefinitions)
        {
            const string schemaTransport = "nsb_t";
            const string schemaPersistence = "nsb_p";
            var connectionStringPersistence = configuration.GetValue<string>(Constants.ConnectionStringPersistenceKey);
            var connectionStringTransport = configuration.GetValue<string>(Constants.ConnectionStringTransportKey);

            var endpointConfiguration = new EndpointConfiguration(endpointName);

            LogManager.Use<DefaultFactory>();

            endpointConfiguration.Conventions().Add(new NewsFeedMessageConvention());

            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.DefaultSchema(schemaTransport);
            transport.ConnectionString(connectionStringTransport);

            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.DisableCache();
            var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
            dialect.Schema(schemaPersistence);
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connectionStringPersistence);
                });

            var routing = transport.Routing();
            routingDefinitions.ForEach(def => routing.RouteToEndpoint(def.assembly, def.endpointName));

            var outbox = endpointConfiguration.EnableOutbox();
            outbox.KeepDeduplicationDataFor(TimeSpan.FromDays(14));

            endpointConfiguration.EnableInstallers();

            return endpointConfiguration;
        }
    }

    public class NewsFeedMessageConvention : IMessageConvention
    {
        public bool IsMessageType(Type type) => type.Namespace is not null && type.Namespace.EndsWith("Messages");

        public bool IsCommandType(Type type) => type.Namespace is not null && type.Namespace.EndsWith("Commands");

        public bool IsEventType(Type type) => type.Namespace is not null && type.Namespace.EndsWith("Events");

        public string Name { get; } = "NewsFeed message convention";
    }
}