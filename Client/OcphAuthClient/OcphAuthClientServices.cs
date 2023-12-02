
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Morris.Blazor.Validation;
using OcphApiAuth.Client;

namespace Client;

public static class OcphAuthClientServices
{
    public static IServiceCollection AddOcphAuthClient(this IServiceCollection services)
    {
        services.AddAuthorizationCore();
        services.AddBlazoredLocalStorage();
        services.AddScoped<OcphAuthStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<OcphAuthStateProvider>());
        services.AddFormValidation(config => config.AddFluentValidation(typeof(Client.App).Assembly));
        services.AddScoped<IAccountService, AccountService>();
        return services;

    }


}
