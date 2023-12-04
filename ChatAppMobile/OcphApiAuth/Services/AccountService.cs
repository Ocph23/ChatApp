using ChatAppMobile;
using Client.OcphAuthClient.Models;
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
    }
}
