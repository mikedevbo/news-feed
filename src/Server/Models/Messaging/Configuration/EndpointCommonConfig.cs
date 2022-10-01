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
            var endpointConfiguration = new EndpointConfiguration(endpointName);

            LogManager.Use<DefaultFactory>();

            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.DisableCache();
            var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
            dialect.Schema("nsb");
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    var connectionString = configuration.GetValue<string>("ConnectionStrings:NsbPersistence");
                    return new SqlConnection(connectionString);
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