using Blazored.LocalStorage;
using Client.OcphAuthClient.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;

namespace OcphApiAuth.Client
{
    public class AccountService : IAccountService
    {
        private readonly NavigationManager navigationManager;
        private HttpClient httpClient;
        private readonly AuthenticationStateProvider authStateProvider;
        private readonly ILocalStorageService localStorage;

        public AccountService(
            NavigationManager _navigationManager,
            AuthenticationStateProvider _authStateProvider,
            ILocalStorageService _localStorage,
            HttpClient _clientFactory)
        {
            navigationManager = _navigationManager;
            authStateProvider = _authStateProvider;
            localStorage = _localStorage;
            httpClient = _clientFactory;
        }
        public async Task<AuthenticateResponse> Login(LoginRequest model)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/account/login", new LoginRequest(model.UserName, model.Password));
                if (response.IsSuccessStatusCode)
                {
                    var contentString = await response.Content.ReadAsStringAsync();
                    AuthenticateResponse result = await response.GetResultAsync<AuthenticateResponse>();
                    if (result != null)
                    {
                        await localStorage.SetItemAsync("token", result.Token);
                        await localStorage.SetItemAsync("userName", result.UserName);
                        await localStorage.SetItemAsync("email", result.Email);
                        await authStateProvider.GetAuthenticationStateAsync();
                        return result;
                    }
                }
                throw new SystemException(await response.Error());
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task Logout()
        {
            await localStorage.RemoveItemAsync("token");
            await localStorage.RemoveItemAsync("userName");
            await localStorage.RemoveItemAsync("email");
            var result = await authStateProvider.GetAuthenticationStateAsync();
            navigationManager.NavigateTo("/");
        }

        public async Task<AuthenticateResponse> Register(RegisterRequest register)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/account/register", register);
                if (response.IsSuccessStatusCode)
                {
                    var contentString = await response.Content.ReadAsStringAsync();
                    AuthenticateResponse result = await response.GetResultAsync<AuthenticateResponse>();
                    if (result != null)
                    {
                        await localStorage.SetItemAsync("token", result.Token);
                        await localStorage.SetItemAsync("userName", result.UserName);
                        await localStorage.SetItemAsync("email", result.Email);
                        await authStateProvider.GetAuthenticationStateAsync();
                        return result;
                    }
                }
                throw new SystemException(await response.Error());
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }
    }
}
