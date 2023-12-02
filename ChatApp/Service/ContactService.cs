using ChatApp.Models;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace ChatApp.Service
{
    public interface IContactService
    {
        Task<Contact> Get(string? userid);
        Task<TemanDTO> AddTeman(string userid, string temanId);
        Task<bool> DeleteTeman(string userid, string temanId);
        Task<GroupDTO> CreateGroup(string userid, GroupDTO group);
        Task<bool> AddAnggota(int groupid, string userId);
        Task<TemanDTO> AddTemanByUserName(string userid, string temanId);
        Task<GroupDTO> GetGroup(int groupid);
    }


    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext dbcontext;

        public ContactService(ApplicationDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public Task<bool> AddAnggota(int groupid, string userId)
        {
            try
            {
                var newAnggota = dbcontext.Users.FirstOrDefault(x => x.Id == userId);
                ArgumentNullException.ThrowIfNull(newAnggota, "Calon Anggota Tidak Ditemuka.");
                var group = dbcontext.Group.Include(x => x.Anggota).SingleOrDefault(x => x.Id == groupid);
                ArgumentNullException.ThrowIfNull(group, "Data Group Tidak Ditemukan. ");
                group.Anggota.Add(new AnggotaGroup { Anggota = newAnggota, Keanggotaan = KeanggotaanGroup.Anggota, TanggalBergabung = DateTime.Now.ToUniversalTime() });
                dbcontext.SaveChanges();
                return Task.FromResult(true);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<TemanDTO> AddTeman(string userid, string temanId)
        {
            try
            {
                var pertemanan = new Pertemanan
                {
                    TanggalBerteman = DateTime.Now.ToUniversalTime(),
                    Teman = new ApplicationUser { Id = temanId },
                    User = new ApplicationUser { Id = userid }
                };
                dbcontext.Pertemanan.Add(pertemanan);
                dbcontext.SaveChanges();
                return Task.FromResult(new TemanDTO { UserId = temanId, Email = pertemanan.Teman.Email, Nama = pertemanan.Teman.Name });
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<TemanDTO> AddTemanByUserName(string userid, string userName)
        {
            try
            {
                var newTeman = dbcontext.Users.FirstOrDefault(x => x.UserName == userName || x.Email == userName);

                var exist = dbcontext.Pertemanan.Where(x => x.UserId == userid || x.TemanId == userid);

                var xxx = exist.Where(x => x.TemanId == newTeman.Id || x.UserId == newTeman.Id);

                if (xxx.Any())
                {
                    throw new System.Exception($"{userName}/{newTeman.Name} sudah menjadi teman Anda !");
                }


                ArgumentNullException.ThrowIfNull(newTeman, "Kontak tidak ditemukan.");
                var pertemanan = new Pertemanan
                {
                    TanggalBerteman = DateTime.Now.ToUniversalTime(),
                    Teman = newTeman,
                    User = new ApplicationUser { Id = userid }
                };

                dbcontext.Entry(pertemanan.Teman).State = EntityState.Unchanged;
                dbcontext.Entry(pertemanan.User).State = EntityState.Unchanged;

                dbcontext.Pertemanan.Add(pertemanan);
                dbcontext.SaveChanges();
                return Task.FromResult(new TemanDTO { UserId = newTeman.Id, Email = pertemanan.Teman.Email, Nama = pertemanan.Teman.Name });
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<GroupDTO> CreateGroup(string userid, GroupDTO groupDTO)
        {
            try
            {
                var user = dbcontext.Users.FirstOrDefault(x => x.Id == userid);
                ArgumentNullException.ThrowIfNull(user, "Akun tidak ditemukan !");
                var g = new Group() { Nama = groupDTO.NameGroup, Deskripsi = groupDTO.Description, Pembuat = user, TanggalBuat = DateTime.Now.ToUniversalTime() };
                g.Anggota.Add(new AnggotaGroup { Anggota = user, Keanggotaan = KeanggotaanGroup.Admin, TanggalBergabung = DateTime.Now.ToUniversalTime() });
                dbcontext.Add(g);
                dbcontext.SaveChanges();
                var result = new GroupDTO
                {
                    Id = g.Id,
                    NameGroup = g.Nama,
                    Description = g.Deskripsi,
                    Owner = g.Pembuat.Name,
                    Created = g.TanggalBuat
                };
                return Task.FromResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> DeleteTeman(string userid, string temanId)
        {
            try
            {
                var teman = dbcontext.Pertemanan.Where(x => x.UserId == userid).Include(x => x.Teman).FirstOrDefault(x => x.Teman.Id == temanId);
                ArgumentNullException.ThrowIfNull(teman, "Data Pertemanan tidak ditemukan.");
                dbcontext.Pertemanan.Remove(teman);
                dbcontext.SaveChanges();
                return Task.FromResult(true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<Contact> Get(string userid)
        {
            try
            {
                var user = dbcontext.Users.FirstOrDefault(x => x.Id == userid);
                ArgumentNullException.ThrowIfNull(user, "Akun tidak ditemukan !");
                var contact = new Contact(userid, user.Name, user.UserName, user.Email);
                var temans = dbcontext.Pertemanan.Include(x => x.Teman)
                    .Where(x => x.UserId == userid || x.Teman.Id == userid).Select(x =>
                    new TemanDTO { Nama = x.Teman.Name, UserId = x.Teman.Id, Email = x.Teman.Email });
                if (temans.Any())
                {
                    contact.Frients = temans.ToList();
                }

                var anggotaGroup = from x in dbcontext.Group.Include(x => x.Anggota)
                                   .ThenInclude(x => x.Anggota).SelectMany(x => x.Anggota).Where(x => x.Anggota.Id == userid)
                                   select x;

                var group = from a in anggotaGroup
                            join g in dbcontext.Group.Include(x => x.Pembuat) on a.GroupId equals g.Id
                            select new GroupDTO
                            {
                                Id = g.Id,
                                NameGroup = g.Nama,
                                Description = g.Deskripsi,
                                Owner = g.Pembuat.Name,
                                Created = g.TanggalBuat
                            };

                if (group.Any())
                {
                    contact.Groups = group.ToList();
                }

                return Task.FromResult(contact);
            }
            catch (Exception)
            {

                throw;
            }
        }

       
        public Task<GroupDTO> GetGroup(int groupid)
        {
            try
            {
                var anggotaGroup = dbcontext.Group
                    .Include(x => x.Pembuat)
                    .Include(x => x.Anggota)
                    .ThenInclude(x => x.Anggota)
                    .Where(x => x.Id == groupid).FirstOrDefault();

                ArgumentNullException.ThrowIfNull(anggotaGroup, "Group Tidak Ditemukan");

                var groupDTO = new GroupDTO
                {
                    Created = anggotaGroup.TanggalBuat,
                    Description = anggotaGroup.Deskripsi,
                    NameGroup = anggotaGroup.Nama,
                    Owner = anggotaGroup.Pembuat.Name,
                    Id = anggotaGroup.Id,
                    Anggota =anggotaGroup.Anggota.ToAnggotaDTO()
                };

                return Task.FromResult(groupDTO);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
