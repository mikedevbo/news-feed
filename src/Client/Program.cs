using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using NewsFeed.Client;
using NewsFeed.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddSingleton<StateContainer>();
builder.Services.AddScoped(sp => new NewsFeedTwitterApiClient(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }));

await builder.Build().RunAsync();
