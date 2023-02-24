using MediatR;
using Microsoft.AspNetCore.Http;
using NewsFeed.Server;
using NewsFeed.Server.Twitter.Database;
using NewsFeed.Server.Twitter.ExternalApi;
using NewsFeed.Server.Twitter.Messaging.Sagas.DownloadTweetsSaga.Commands;
using NewsFeed.Shared.Twitter;
using NewsFeed.Shared.Twitter.Commands;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using NServiceBus.TransactionalSession;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
Constants.Initialize(config);

builder.Logging.AddLog4Net();

builder.Host.UseNServiceBus(context =>
 {
     var endpointName = typeof(Program).Assembly.GetName().Name!;
     var endpointConfig = EndpointCommonConfig.Get(
         endpointName,
         config,
         new List<(Assembly, string)>
         {
             (typeof(StartDownloadingTweets).Assembly, endpointName),
             (typeof(StartDownloadingTweetsForUser).Assembly, endpointName)
         });

     //endpointConfig.RegisterComponents(c =>
     //{
     //    c.ConfigureComponent(b =>
     //    {
     //        var session = b.Build<ISqlStorageSession>();
     //        var repository = new TwitterRepository(
     //            session.Connection,
     //            session.Transaction);

     //        return repository;
     //    }, DependencyLifecycle.InstancePerUnitOfWork);
     //});

     return endpointConfig;
 });

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<ITwitterRepository, TwitterRepository>(b =>
{
    var session = b.GetRequiredService<ISqlStorageSession>();
    var repository = new TwitterRepository(
        session.Connection,
        session.Transaction);

    return repository;
});

if (config.GetValue<bool>("IsUseFake"))
{
    builder.Services.AddScoped<ITwitterApiClient, TwitterApiClientFake>();
}
else
{
    builder.Services.AddScoped<ITwitterApiClient>(provider =>
        new TwitterApiClient(config.GetValue<string>("TwitterToken")));
}

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.MapPost($"/{typeof(GetMenuRequest).Name}", async (GetMenuRequest request, IMediator mediator) => await mediator.Send(request));
app.MapPost($"/{typeof(GetTweetsRequest).Name}", async (GetTweetsRequest request, IMediator mediator) => await mediator.Send(request));
app.MapPost($"/{typeof(SetReadStateRequest).Name}", async (SetReadStateRequest request, IMediator mediator) => await mediator.Send(request));
app.MapPost($"/{typeof(SetFavoriteStateRequest).Name}", async (SetFavoriteStateRequest request, IMediator mediator) => await mediator.Send(request));

app.MapPost("/twitter/tweets/startdownloading", async (
    ITransactionalSession messageSession,
    ITwitterRepository twitterRepository,
    StartDownloadingTweets command) =>
{
    Console.WriteLine("execute StartDownloadingTweets " + System.Text.Json.JsonSerializer.Serialize(command));

    await messageSession.Open(new SqlPersistenceOpenSessionOptions());

    ////TODO: add logic
    await twitterRepository.SetTweetsDownloadingState(command.Users[0].UserId, true);
    await messageSession.Send(command);

    await messageSession.Commit();

    return Results.Ok();
});

app.Run();
