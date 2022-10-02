using NewsFeed.Server.Models;
using NewsFeed.Server.Models.Messaging.Commands;
using NewsFeed.Server.Models.Messaging.Configuration;
using NewsFeed.Server.Models.Twitter;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Logging.AddLog4Net();

builder.Host.UseNServiceBus(context =>
 {
     var endpointName = typeof(Program).Assembly.GetName().Name!;
     var endpointConfig = EndpointCommonConfig.Get(
         endpointName,
         config,
         new List<(Assembly, string)>
         {
             (typeof(DownloadTweets).Assembly, endpointName)
         });

     endpointConfig.RegisterComponents(c =>
     {
         c.ConfigureComponent(b =>
         {
             var session = b.Build<ISqlStorageSession>();
             var repository = new TwitterRepository(
                 session.Connection,
                 session.Transaction);

             return repository;
         }, DependencyLifecycle.InstancePerUnitOfWork);
     });

     return endpointConfig;
 });

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<ITwitterSelfConnectionRepository, TwitterSelfConnectionRepository>();

if (config.GetValue<bool>("IsUseFake"))
{
    builder.Services.AddScoped<ITwitterApiClient, TwitterApiClientFake>();
}
else
{
    builder.Services.AddScoped<ITwitterApiClient>(provider =>
        new TwitterApiClient(config.GetValue<string>("TwitterToken")));
}


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

app.Run();
