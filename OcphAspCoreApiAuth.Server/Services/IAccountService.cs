using OcphApiAuth;

namespace OcphApiAuth
{
    public interface IAccountService<T>
    {
        Task<T> FindUserById(string id);
        Task<AuthenticateResponse> Login(string userName, string password);
        Task<AuthenticateResponse> Register(T requst, string role, string password);
        Task AddUserRole(string v, T user);
        Task<T> FindUserByUserName(string userName);
        Task<T> FindUserByEmail(string email);
        Task<IEnumerable<T>> GetUsers();
        Task<bool> UpdateUser(T user);  
    }

}
