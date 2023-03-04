using MediatR;
using NewsFeed.Server.Twitter.Messaging.DownloadTweetsPolicy.Commands;
using NewsFeed.Shared.Twitter;
using NServiceBus;
using NServiceBus.TransactionalSession;

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
