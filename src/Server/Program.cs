using MediatR;
using NewsFeed.Server;
using NewsFeed.Server.Twitter.ExternalApi;
using NewsFeed.Server.Twitter.Messaging.DownloadTweetsPolicy.Commands;
using NewsFeed.Shared.Twitter;
using NServiceBus;
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
             (typeof(StartDownloadingTweets).Assembly, endpointName)
         });

     return endpointConfig;
 });

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

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
app.MapPost($"/{typeof(StartDownloadingTweetsRequest).Name}", async (StartDownloadingTweetsRequest request, IMediator mediator) => await mediator.Send(request));
app.MapPost($"/{typeof(GetDownloadingTweetsState).Name}", async (GetDownloadingTweetsState request, IMediator mediator) => await mediator.Send(request));

app.Run();
