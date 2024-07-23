using ChatApp.Models;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Contracts;
using System.Runtime.Serialization;

namespace ChatApp.Service
{


    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext dbcontext;

        public ContactService(ApplicationDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public Task<TemanDTO> AddAnggota(int groupid, string email)
        {
            try
            {
                var newAnggota = dbcontext.Users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
                ArgumentNullException.ThrowIfNull(newAnggota, "Calon Anggota Tidak Ditemuka.");
                var group = dbcontext.Group.Include(x => x.Anggota.Where(x => x.Anggota.Id == newAnggota.Id)).SingleOrDefault(x => x.Id == groupid);
                ArgumentNullException.ThrowIfNull(group, "Data Group Tidak Ditemukan. ");
                if (group.Anggota.Count > 0)
                {
                    throw new SystemException("User Sudah Menjadi Anggota");
                }
                group.Anggota.Add(new AnggotaGroup { Anggota = newAnggota, Keanggotaan = KeanggotaanGroup.Anggota, TanggalBergabung = DateTime.Now.ToUniversalTime() });
                dbcontext.SaveChanges();
                return Task.FromResult(new TemanDTO { TemanId = newAnggota.Id, Email = newAnggota.Email, Nama = newAnggota.Name });
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
                return Task.FromResult(new TemanDTO { TemanId = temanId, Email = pertemanan.Teman.Email, Nama = pertemanan.Teman.Name });
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
                var newTeman = dbcontext.Users.FirstOrDefault(x => x.UserName.ToLower() == userName || x.Email.ToLower() == userName || x.PhoneNumber.ToLower() == userName);

                ArgumentNullException.ThrowIfNull(newTeman, "Data User Tidak Ditemukan ");

                var exist = dbcontext.Pertemanan.Where(x => (x.UserId == userid && x.TemanId == newTeman.Id) || (x.UserId == newTeman.Id && x.TemanId == userid));
                if (exist.Any())
                {
                    throw new System.Exception($"{userName}/{newTeman.Name} sudah menjadi teman Anda !");
                }

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
                return Task.FromResult(new TemanDTO { TemanId = newTeman.Id, Email = pertemanan.Teman.Email, Nama = pertemanan.Teman.Name });
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
                foreach (var anggota in groupDTO.Anggota)
                {
                    var xUser = dbcontext.Users.FirstOrDefault(x => x.Id == anggota.TemanId);
                    if (xUser != null)
                    {
                        g.Anggota.Add(new AnggotaGroup
                        {
                            Anggota = xUser,
                            Keanggotaan = anggota.Keanggotaan,
                            TanggalBergabung = DateTime.Now.ToUniversalTime()
                        });

                    }
                }
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
            catch (Exception ex)
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
                var contact = new Contact(userid, user.Name, user.UserName, user.Email, user.Photo);
                var temans = dbcontext.Pertemanan
                    .Include(x => x.Teman)
                    .Include(x => x.User)
                    .Where(x => x.UserId == userid || x.Teman.Id == userid).Select(x =>
                    new TemanDTO
                    {
                        Nama = x.UserId == userid ? x.Teman.Name : x.User.Name,
                        TemanId = x.UserId == userid ? x.Teman.Id : x.UserId,
                        Email = x.UserId == userid ? x.Teman.Email : x.User.Email,
                        Photo = x.User.Photo
                    });

                if (temans.Any())
                {
                    contact.Friends = temans.ToList();
                }

                var anggotaGroup = dbcontext.AnggotaGroup.Include(x => x.Anggota).Where(x => x.Anggota.Id == userid);


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
                    Anggota = anggotaGroup.Anggota.ToAnggotaDTO()
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
