using ChatAppMobile;
using Client.OcphAuthClient.Models;
using Microsoft.Win32;
using Shared;
using System.Net.Http.Json;

namespace OcphApiAuth.Client
{
    public class AccountService : IAccountService
    {
        private HttpClient httpClient;

        public AccountService(HttpClient _clientFactory)
        {
            httpClient = _clientFactory;
        }

        public async Task<UserDTO> GetProfile()
        {
            try
            {
                var response = await httpClient.GetAsync("api/account/profile");
                if (response.IsSuccessStatusCode)
                {
                    var contentString = await response.Content.ReadAsStringAsync();
                    UserDTO result = await response.GetResultAsync<UserDTO>();
                    return result;
                }
                throw new SystemException(await response.Error());
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
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

                    var service = ServiceHelper.GetService<OcphAuthStateProvider>();
                    var claim = await service.GetAuthenticationStateAsync(result.Token);
                    if (!claim.Identity.IsAuthenticated || !claim.IsInRole("User"))
                    {
                        throw new SystemException("Not Have Access !");
                    }
                    return result;
                }
                throw new SystemException(await response.Error());
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task Logout()
        {
            return Task.CompletedTask;
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

        public async Task<string> RequestPublicKey(string? temanId)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/account/publickey/{temanId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                throw new SystemException(await response.Error());
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<bool> UpdateUser(UserDTO user)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"api/account/profile/{user.Id}", user);
                if (response.IsSuccessStatusCode)
                {
                    return true;
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
