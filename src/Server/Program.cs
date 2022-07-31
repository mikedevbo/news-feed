using NewsFeed.Server.Models.Messaging.Commands;
using NServiceBus;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNServiceBus(context =>
 {
     const string endpointName = "NewsFeed.Server";
     var endpointConfiguration = new EndpointConfiguration(endpointName);
     var transport = endpointConfiguration.UseTransport<LearningTransport>();
     endpointConfiguration.UsePersistence<LearningPersistence>();

     var routing = transport.Routing();
     routing.RouteToEndpoint(typeof(DownloadNewTweets).Assembly, endpointName);

     endpointConfiguration.MakeInstanceUniquelyAddressable("1");
     endpointConfiguration.EnableCallbacks();

     return endpointConfiguration;
 });

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

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
