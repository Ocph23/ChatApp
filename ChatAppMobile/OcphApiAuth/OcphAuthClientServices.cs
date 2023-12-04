
using MarampaApp.Client.OcphAuthClient;
using Microsoft.Extensions.DependencyInjection;
using OcphApiAuth.Client;

namespace Client;

public static class OcphAuthClientServices
{
    public static IServiceCollection AddOcphAuthClient(this IServiceCollection services,string urlTarget)
    {
        services.AddTransient<HttpClientAuthenticationHandler>();
        services.AddHttpClient("ChatApp", client => client.BaseAddress = new Uri(urlTarget))
        .AddHttpMessageHandler<HttpClientAuthenticationHandler>();
        services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ChatApp"));
        services.AddScoped<OcphAuthStateProvider>();
        services.AddScoped<IAccountService, AccountService>();
        return services;

    }


}
