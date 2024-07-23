using ChatAppMobile.Models;
using OcphApiAuth.Client;
using Shared;
using Shared.Contracts;
using System.Net.Http.Json;
using System.Text.Json;
using Contact = Shared.Contact;

namespace ChatAppMobile.Services
{
    public class ContactService : IContactService
    {
        private readonly HttpClient httpClient;
        private string controller = "api/Contact";
        private MobileContact _contact;

        public ContactService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<TemanViewModel> AddAnggota(int groupid, string email)
        {
            try
            {
                var response = await httpClient.GetAsync($"{controller}/addmember/{groupid}/{email}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.GetResultAsync<TemanViewModel>();
                }
                throw new SystemException(await response.Error());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<TemanViewModel> AddTeman(string userid, string email)
        {
            try
            {
                var response = await httpClient.GetAsync($"{controller}/addmember/{email}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.GetResultAsync<TemanViewModel>();
                }
                throw new SystemException(await response.Error());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TemanViewModel> AddTemanByUserName(string username)
        {
            try
            {
                var response = await httpClient.GetAsync($"{controller}/addtemanbyuserName/{username}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.GetResultAsync<TemanViewModel>();
                }
                throw new SystemException(await response.Error());
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

                var result = JsonSerializer.Serialize(group);

                var response = await httpClient.PostAsJsonAsync($"{controller}/creategroup", group);
                if (response.IsSuccessStatusCode)
                {
                    return await response.GetResultAsync<GroupDTO>();
                }
                throw new SystemException();
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
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


        public async Task<MobileContact> Get()
        {
            try
            {
                if (_contact == null)
                {
                    var response = await httpClient.GetAsync($"{controller}");
                    if (response.IsSuccessStatusCode)
                    {
                        _contact = await response.GetResultAsync<MobileContact>();
                    }
                    else
                    {
                        throw new SystemException(await response.Error());
                    }
                }
                return _contact;
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
                throw new SystemException(await response.Error());
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
