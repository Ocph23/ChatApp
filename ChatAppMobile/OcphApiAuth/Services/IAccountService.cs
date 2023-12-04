
using Client.OcphAuthClient.Models;

namespace OcphApiAuth.Client
{
    public interface IAccountService
    {
        Task<AuthenticateResponse> Login(LoginRequest login);
        Task<AuthenticateResponse> Register(RegisterRequest register);
        Task Logout();
    }
}
