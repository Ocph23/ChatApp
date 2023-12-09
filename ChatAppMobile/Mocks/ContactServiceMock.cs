using ChatAppMobile.Services;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contact = Shared.Contact;

namespace ChatAppMobile.Mocks
{
    public class ContactServiceMock : IContactService
    {
        public Task<bool> AddAnggota(int groupid, string userId)
        {
            return Task.FromResult(true);
        }

        public Task<TemanDTO> AddTeman(string userid, string temanId)
        {
            return Task.FromResult(new TemanDTO { Email = "elish@gmail.com", Nama = "Elish", TemanId = "1", });
        }

        public Task<TemanDTO> AddTemanByUserName(string userName)
        {
            return Task.FromResult(new TemanDTO { Email = "elish@gmail.com", Nama = "Elish", TemanId = "1", });
        }

        public Task<GroupDTO> CreateGroup(string userid, GroupDTO group)
        {
            return Task.FromResult(new GroupDTO
            {
                Description = "Group A",
                NameGroup = "Group A",
                Anggota = new TemanDTO[]
                    { new TemanDTO { Email = "elish@gmail.com", Nama = "Elish", TemanId = "1", } }
            });
        }

        public Task<bool> DeleteTeman(string userid, string temanId)
        {
            return Task.FromResult(true);
        }

        public Task<Shared.Contact> Get()
        {
            var contact = new Contact
            {
                Email = "ocph23@gmail.com",
                Name = "Yoseph Kungkung",
                UserId = "1",
                UserName = "ocph23",
                Friends = new TemanDTO[] { new TemanDTO { Email = "elish@gmail.com", Nama = "Elish", TemanId = "1", },
                new TemanDTO { Email = "elish@gmail.com", Nama = "Elish", TemanId = "1", },
                new TemanDTO { Email = "elish@gmail.com", Nama = "Elish", TemanId = "1", }},
                Groups = new GroupDTO[] {
                    new GroupDTO { Description="Group A", NameGroup="Group A", Anggota= new TemanDTO[]
                    { new TemanDTO { Email = "elish@gmail.com", Nama = "Elish", TemanId = "1", } }}}
            };

            return Task.FromResult(contact);
        }

        public Task<GroupDTO> GetGroup(int groupid)
        {
            var x = new GroupDTO
            {
                Description = "Group A",
                NameGroup = "Group A",
                Anggota = new TemanDTO[]
                     { new TemanDTO { Email = "elish@gmail.com", Nama = "Elish", TemanId = "1", } }
            };
            return Task.FromResult(x);
        }
    }
}
