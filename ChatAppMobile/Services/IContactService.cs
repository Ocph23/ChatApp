using Shared;
using System.Threading.Tasks;
using Contact = Shared.Contact;

namespace ChatAppMobile.Services
{
    public interface IContactService
    {
        Task<Contact> Get();
        Task<TemanDTO> AddTeman(string userid, string temanId);
        Task<bool> DeleteTeman(string userid, string temanId);
        Task<GroupDTO> CreateGroup(string userid, GroupDTO group);
        Task<bool> AddAnggota(int groupid, string userId);
        Task<TemanDTO> AddTemanByUserName(string userName);
        Task<GroupDTO> GetGroup(int groupid);
    }
}
