using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using NewsFeed.Shared.Twitter;
using System.Threading;

namespace NewsFeed.Server.Twitter.Mediating
{
    public class StartDownloadingTweetsRequestHandler : IRequestHandler<StartDownloadingTweetsRequest>
    {
        private readonly IConfiguration configuration;

        public StartDownloadingTweetsRequestHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task Handle(StartDownloadingTweetsRequest request, CancellationToken cancellationToken)
        {
            return this.Command(request);
        }

        public async Task Command(StartDownloadingTweetsRequest request)
        {
            await Console.Out.WriteLineAsync("users count " + request.Users.Count);

            //    await messageSession.Open(new SqlPersistenceOpenSessionOptions());

            //    ////TODO: add logic
            //    await twitterRepository.SetTweetsDownloadingState(command.Users[0].UserId, true);
            //    await messageSession.Send(command);

            //    await messageSession.Commit();
        }
    }
}
