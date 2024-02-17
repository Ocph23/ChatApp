
using Client.OcphAuthClient.Models;
using Shared;

namespace OcphApiAuth.Client
{
    public interface IAccountService
    {
        Task<AuthenticateResponse> Login(LoginRequest login);
        Task<AuthenticateResponse> Register(RegisterRequest register);
        Task Logout();
        Task<UserDTO> GetProfile();
        Task<bool> UpdateUser(UserDTO user);
        Task<string> RequestPublicKey(string? temanId);
    }
}
