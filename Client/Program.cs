using Client;
using Client.Models;
using Client.Services;
using MarampaApp;
using MarampaApp.Client.OcphAuthClient;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shared;
using Shared.Contracts;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddTransient<HttpClientAuthenticationHandler>();
builder.Services.AddHttpClient("ChatApp",
    client => client.BaseAddress = new Uri(Helper.ServerURL))
    .AddHttpMessageHandler<HttpClientAuthenticationHandler>();
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ChatApp"));
builder.Services.AddOcphAuthClient();
builder.Services.AddScoped<IMessageService,MessageService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ChatClient>();
await builder.Build().RunAsync();
