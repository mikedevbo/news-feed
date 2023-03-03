using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga.Commands;
using NewsFeed.Shared.Twitter;
using NServiceBus;
using NServiceBus.TransactionalSession;
using System.Threading;
using System.Transactions;

namespace NewsFeed.Server.Twitter.Mediating
{
    public class StartDownloadingTweetsRequestHandler : IRequestHandler<StartDownloadingTweetsRequest>
    {
        private readonly IConfiguration configuration;
        private readonly ITransactionalSession session;

        public StartDownloadingTweetsRequestHandler(
            IConfiguration configuration,
            ITransactionalSession session)
        {
            this.configuration = configuration;
            this.session = session;
        }

        public Task Handle(StartDownloadingTweetsRequest request, CancellationToken cancellationToken)
        {
            return this.Command(request);
        }

        public async Task Command(StartDownloadingTweetsRequest request)
        {
            //var data = request.Users.Select(u => new StartDownloadingTweets.UserData(u.UserId, u.TwitterUserId)).ToList();
            //var data = request.Users.ToDictionary(u => u.UserId, u => u.TwitterUserId);
            var data = request.Users.Select(u => (u.UserId, u.TwitterUserId)).ToList();
            var command = new StartDownloadingTweets(data);

            await this.session.Open(new SqlPersistenceOpenSessionOptions());

            var storageSession = this.session.SynchronizedStorageSession.SqlPersistenceSession();
            await TwitterCommon.SetUserIsDownloadingTweetsState(
                storageSession.Connection,
                storageSession.Transaction,
                request.Users.Select(u => u.UserId).ToList(),
                true);

            await this.session.Send(command);

            await this.session.Commit();
        }
    }
}
