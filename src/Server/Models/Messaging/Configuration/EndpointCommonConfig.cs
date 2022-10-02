using NServiceBus.Logging;
using NServiceBus;
using NewsFeed.Server.Models.Messaging.Commands;
using System.Reflection;
using System.Data.SqlClient;
using NServiceBus.Outbox;

namespace NewsFeed.Server.Models.Messaging.Configuration
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
}