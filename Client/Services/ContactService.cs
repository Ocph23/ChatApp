using OcphApiAuth.Client;
using Shared;
using Shared.Contracts;
using System.Net.Http.Json;

namespace Client.Services
{
    public class ContactService : IContactService
    {
        private readonly HttpClient httpClient;
        private string controller = "api/contact";

        public ContactService(HttpClient httpClient) {
            this.httpClient = httpClient;
        }
        public Task<bool> AddAnggota(int groupid, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<TemanDTO> AddTeman(string userid, string temanId)
        {
            throw new NotImplementedException();
        }

        public async Task<TemanDTO> AddTemanByUserName(string username)
        {
            try
            {
                var response = await httpClient.GetAsync($"{controller}/addtemanbyuser/{username}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.GetResultAsync<TemanDTO>();
                }
                throw new SystemException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupDTO> CreateGroup(string userid, GroupDTO group)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{controller}/creategroup",group);
                if (response.IsSuccessStatusCode)
                {
                    return await response.GetResultAsync<GroupDTO>();
                }
                throw new SystemException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteTeman(string userid, string temanId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"{controller}/teman/{temanId}");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                throw new SystemException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Contact> Get()
        {
            try
            {
                var response = await httpClient.GetAsync($"{controller}");
                if(response.IsSuccessStatusCode)
                {
                    return await response.GetResultAsync<Contact>();
                }
                throw new SystemException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupDTO> GetGroup(int groupid)
        {
            try
            {
                var response = await httpClient.GetAsync($"{controller}/group/{groupid}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.GetResultAsync<GroupDTO>();
                }
                throw new SystemException();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
