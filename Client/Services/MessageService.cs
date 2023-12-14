using OcphApiAuth.Client;
using Shared;
using Shared.Contracts;
using System.Net.Http;
using System.Net.Http.Json;

namespace Client.Services
{
    public class MessageService : IMessageService
    {
        private readonly HttpClient httpClient;
        private string controller = "api/message";

        public MessageService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<MessageGroup>> GetGroupMessage(int groupId)
        {
            try
            {
                var response = await httpClient.GetAsync($"{controller}/group/{groupId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.GetResultAsync<IEnumerable<MessageGroup>>();
                }
                throw new SystemException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<MessagePrivate>> GetPrivateMessage(string? userid1, string userid2)
        {
            try
            {
                var response = await httpClient.GetAsync($"{controller}/private/{userid1}/{userid2}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.GetResultAsync<IEnumerable<MessagePrivate>>();
                }
                throw new SystemException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MessageGroup> PostGroupMessage(MessageGroup mesage)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{controller}/group", mesage);
                if (response.IsSuccessStatusCode)
                {
                    return await response.GetResultAsync<MessageGroup>();
                }
                throw new SystemException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MessagePrivate> PostPrivateMessage(MessagePrivate message)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{controller}/private", message);
                if (response.IsSuccessStatusCode)
                {
                    return await response.GetResultAsync<MessagePrivate>();
                }
                throw new SystemException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> ReadMassage(string? temanId, string myId)
        {
            throw new NotImplementedException();
        }
    }
}
