using Shared;
using System.Threading.Tasks;

namespace ChatApp.Service
{
    public interface IContactService
    {
        Task<Contact> Get(string userid);
        Task<TemanDTO> AddTeman(string userid, string temanId);
        Task<bool> DeleteTeman(string userid, string temanId);
        Task<GroupDTO> CreateGroup(string userid, GroupDTO group);
        Task<TemanDTO> AddAnggota(int groupid, string userId);
        Task<TemanDTO> AddTemanByUserName(string userid, string userName);
        Task<GroupDTO> GetGroup(int groupid);
    }
}
