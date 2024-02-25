using MarampaApp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace OcphApiAuth
{
    public class AccountService<T, DB> : IAccountService<T> where T : class where DB : class
    {
        private readonly AppSettings _appSettings;
        private readonly UserManager<T> userManager;
        private readonly SignInManager<T> signInManager;
        private readonly DB dbcontext;

        public AccountService(IOptions<AppSettings> appSettings,
            UserManager<T>
            _userManager, SignInManager<T> _signInManager, DB _dbcontext)
        {
            _appSettings = appSettings.Value;
            userManager = _userManager;
            signInManager = _signInManager;
            dbcontext = _dbcontext;
        }

        public async Task<AuthenticateResponse> Login(string userName, string password)
        {
            try
            {
                var result = await signInManager.PasswordSignInAsync(userName.ToUpper(), password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    T? user = await userManager.FindByEmailAsync(userName.ToUpper());
                    ArgumentNullException.ThrowIfNull(user);
                    var roles = await userManager.GetRolesAsync(user);
                    var token = await user.GenerateToken(_appSettings, roles);
                    var  privateKey = user.GetType().GetProperty("PrivateKey").GetValue(user, null);
                    return new AuthenticateResponse(userName!, userName!, token, privateKey==null?string.Empty:privateKey.ToString() );

                }
                throw new SystemException($"Your Account {userName} Not Have Access !");
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<AuthenticateResponse> Register(T user, string role, string password)
        {
            try
            {

                var userCreated = await userManager.CreateAsync(user, password);
                if (userCreated.Succeeded)
                {
                    if (!string.IsNullOrEmpty(role))
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                    var token = await user.GenerateToken(_appSettings, new List<string> { role });
                    var userResult = user as IdentityUser;

                    var privateKey = userResult.GetType().GetProperty("PrivateKey").GetValue(userResult, null);
                    return new AuthenticateResponse(userResult.UserName, userResult.Email, token,privateKey.ToString());
                }

                string errorMessage = string.Empty;
                if (userCreated.Errors.Count() > 0)
                {
                    errorMessage = userCreated.Errors.FirstOrDefault().Description;
                }

                throw new SystemException(errorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<T> FindUserById(string id) => await userManager.FindByIdAsync(id);

        public Task AddUserRole(string v, T user)
        {
            throw new NotImplementedException();
        }

        public async Task<T> FindUserByUserName(string userName) => await userManager.FindByNameAsync(userName);

        public Task<IEnumerable<T>> GetUsers()
        {
            throw new NotImplementedException();
        }
        public Task<T> FindUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateUser(T user)
        {
            try
            {
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }


}
