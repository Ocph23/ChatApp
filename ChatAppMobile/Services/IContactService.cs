using ChatAppMobile.Models;
using Shared;

namespace ChatAppMobile.Services
{
    public interface IContactService
    {
        Task<MobileContact> Get();
        Task<TemanViewModel> AddTeman(string userid, string temanId);
        Task<bool> DeleteTeman(string userid, string temanId);
        Task<GroupDTO> CreateGroup(string userid, GroupDTO group);
        Task<bool> AddAnggota(int groupid, string userId);
        Task<TemanViewModel> AddTemanByUserName(string userName);
        Task<GroupDTO> GetGroup(int groupid);
    }
}
