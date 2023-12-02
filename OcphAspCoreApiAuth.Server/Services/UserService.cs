//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity.UI.Services;
//using OcphApiAuth;
//using MarampaApp.Models.Contracts;
//using AutoMapper;
//using MarampaApp.Shared.Models;
//using MarampaApp.Models;

//namespace MarampaApp.Server.Services
//{

//    public class UserService : IUserService
//    {
//        private readonly ApplicationDbContext dbcontext;
//        private readonly UserManager<ApplicationUser> userManager;
//        private readonly IEmailSender emailSender;
//        private readonly IMapper mapper;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        public ApplicationDbContext ApplicationDbContext { get; }

//        public UserService(
//            ApplicationDbContext _dbcontext,
//            UserManager<ApplicationUser> _userManager, IEmailSender _emailSender,
//            IMapper _mapper,
//            IHttpContextAccessor httpContextAccessor)
//        {
//            dbcontext = _dbcontext;
//            userManager = _userManager;
//            emailSender = _emailSender;
//            mapper = _mapper;
//            _httpContextAccessor = httpContextAccessor;
//        }

//        public async Task<UserJemaat> Register(UserJemaat requst)
//        {
//            try
//            {
//                requst.Password = "Password@123";
//                requst.Actived = true;
//                var userExist = await userManager.FindByEmailAsync(requst.Email);
//                if (userExist != null)
//                    throw new SystemException($"User {requst.Email} Sudah Digunakan.");
//                var jemaatHaveAccount = dbcontext.UserJemaat.SingleOrDefault(x => x.JemaatId == requst.Id);
//                if (jemaatHaveAccount != null)
//                    throw new SystemException("User Sudah Ada.");

//                var user = new ApplicationUser(requst.Email) { Email = requst.Email, EmailConfirmed = true };
//                var createResult = await userManager.CreateAsync(user, requst.Password);
//                if (!createResult.Succeeded)
//                {
//                    var error = createResult.Errors.FirstOrDefault();
//                    if (error == null)
//                        throw new SystemException("User Tidak berhasil dibuat");
//                    else
//                        throw new SystemException(error.Description);
//                }
//                await userManager.AddToRolesAsync(user, requst.Roles);
//                requst.UserId = user.Id;
//                if (requst.Jemaat != null)
//                    dbcontext.Entry(requst.Jemaat).State = EntityState.Unchanged;
//                dbcontext.UserJemaat.Add(requst);
//                var saveValue = dbcontext.SaveChanges();
//                if (saveValue <= 0)
//                {
//                    await userManager.DeleteAsync(user);
//                }
//                await emailSender.SendEmailAsync(requst.Email, "New Account", $"User Name {requst.Email} & Password = {requst.Password}");
//                return requst;
//            }
//            catch (Exception ex)
//            {
//                throw new SystemException(ex.Message);
//            }
//        }

//        public async Task<bool> UpdateUser(UserJemaat requst)
//        {
//            try
//            {
//                var data = dbcontext.UserJemaat.SingleOrDefault(x => x.Id == requst.Id);
//                data.Roles = (from userRole in dbcontext.UserRoles.Where(x => x.UserId == data.UserId)
//                              join role in dbcontext.Roles on userRole.RoleId
//                                  equals role.Id
//                              select role.Name).ToList();


//                var user = await userManager.FindByIdAsync(requst.UserId);

//                ///add new
//                foreach (var role in requst.Roles.ToList())
//                {
//                    var dbRole = data.Roles.FirstOrDefault(x => x.ToLower() == role.ToLower());
//                    if (dbRole == null)
//                    {
//                        await userManager.AddToRoleAsync(user, role);
//                    }
//                }


//                ///Delete
//                foreach (var dbRole in data.Roles.ToList())
//                {
//                    var role = requst.Roles.FirstOrDefault(x => x.ToLower() == dbRole.ToLower());
//                    if (role == null)
//                    {
//                        await userManager.RemoveFromRoleAsync(user, dbRole);
//                    }
//                }

//                data.Actived = requst.Actived;
//                dbcontext.SaveChanges();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                throw new SystemException(ex.Message);
//            }


//        }

//        public Task<IEnumerable<UserJemaat>> Get()
//        {
//            var data = (from user in dbcontext.Users
//                        join b in dbcontext.UserJemaat on user.Id equals b.UserId
//                        join j in dbcontext.Jemaat on b.JemaatId equals j.Id
//                        select new UserJemaat()
//                        {
//                            Id = b.Id,
//                            UserId = user.Id,
//                            JemaatId = j.Id,
//                            Jemaat = j,
//                            Email = user.Email,
//                            Actived = b.Actived,
//                            Roles = (from userRole in dbcontext.UserRoles.Where(x => x.UserId == user.Id)
//                                     join role in dbcontext.Roles on userRole.RoleId
//                                         equals role.Id
//                                     select role.Name).ToList()
//                        }).AsEnumerable();

//            return Task.FromResult(data);
//        }


//        public Task<UserJemaat> GetUserJemaatByUserId(string userid)
//        {
//            var data = (from user in dbcontext.Users.Where(x => x.Id == userid)
//                        join b in dbcontext.UserJemaat on user.Id equals b.UserId
//                        join j in dbcontext.Jemaat on b.JemaatId equals j.Id
//                        select new UserJemaat()
//                        {
//                            Id = b.Id,
//                            UserId = user.Id,
//                            Email = user.Email,
//                            JemaatId = j.Id,
//                            Actived = b.Actived,
//                            Roles = (from userRole in dbcontext.UserRoles.Where(x => x.UserId == user.Id)
//                                     join role in dbcontext.Roles on userRole.RoleId
//                                         equals role.Id
//                                     select role.Name).ToList()
//                        }).AsEnumerable();

//            return Task.FromResult(data.FirstOrDefault());
//        }
//        public Task<UserJemaat> GetUserJemaatByUserName(string username)
//        {
//            var data = (from user in dbcontext.Users.Where(x => x.UserName == username || x.Email == username)
//                        join b in dbcontext.UserJemaat on user.Id equals b.UserId
//                        join j in dbcontext.Jemaat on b.JemaatId equals j.Id
//                        select new UserJemaat()
//                        {
//                            Id = b.Id,
//                            UserId = user.Id,
//                            Email = user.Email,
//                            JemaatId = j.Id,
//                            Actived = b.Actived,
//                            Roles = (from userRole in dbcontext.UserRoles.Where(x => x.UserId == user.Id)
//                                     join role in dbcontext.Roles on userRole.RoleId
//                                         equals role.Id
//                                     select role.Name).ToList()
//                        }).AsEnumerable();

//            return Task.FromResult(data.FirstOrDefault());
//        }

//        public Task<Rayon> GetUserRayon(string username)
//        {
//            try
//            {
//                var data = (from user in dbcontext.Users.Where(x => x.UserName.ToLower() == username.ToLower() || x.Email.ToLower() == username.ToLower())
//                            join b in dbcontext.UserJemaat on user.Id equals b.UserId
//                            join j in dbcontext.Jemaat on b.JemaatId equals j.Id
//                            select new UserJemaat()
//                            {
//                                JemaatId = j.Id
//                            }).FirstOrDefault();

//                ArgumentNullException.ThrowIfNull(data);

//                var datas =
//                    from a in dbcontext.AnggotaKeluarga.Where(x => x.JemaatId == data.JemaatId)
//                    join k in dbcontext.DataKeluarga.Include(x => x.Rayon) on a.KeluargaId equals k.Id
//                    select mapper.Map<Rayon>(k.Rayon);

//                return Task.FromResult(datas.FirstOrDefault());
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        public Task<KeluargaDTO> GetKeluargaByUser(string username)
//        {
//            try
//            {
//                var data = (from user in dbcontext.Users.Where(x => x.UserName.ToLower() == username.ToLower() || x.Email.ToLower() == username.ToLower())
//                            join b in dbcontext.UserJemaat on user.Id equals b.UserId
//                            join j in dbcontext.Jemaat on b.JemaatId equals j.Id
//                            select new UserJemaat()
//                            {
//                                JemaatId = j.Id
//                            }).FirstOrDefault();

//                ArgumentNullException.ThrowIfNull(data);

//                var datas =
//                    from a in dbcontext.AnggotaKeluarga.Where(x => x.JemaatId == data.JemaatId)
//                    join k in dbcontext.DataKeluarga
//                    .Include(x => x.AnggotaKeluarga).ThenInclude(x => x.Jemaat)
//                    .Include(x => x.Rayon) on a.KeluargaId equals k.Id
//                    select k;

//                if (datas.Count() <= 0)
//                    throw new Exception("Anda Tidak Terdaftar dalam sebuah Keluarga, Hubungi Administrator");


//                return Task.FromResult(mapper.Map<KeluargaDTO>(datas.FirstOrDefault()));
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }


//    }
//}