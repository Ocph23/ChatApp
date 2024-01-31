using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMobile.Services
{
    public interface IFileServices
    {
        void DownloadFile(string? v);
        Task<string> UploadGroupFile(int groupid, MultipartFormDataContent content);
        Task<string> UploadPrivateFile(string temanId, MultipartFormDataContent content);
    }


    public class FileService : IFileServices
    {
        private readonly HttpClient httpClient;
        private string controller = "api/Files";

        public FileService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string> UploadGroupFile(int groupId,MultipartFormDataContent content)
        {
            try
            {
                ///api / Files / uploadgroupfile ? groupid = 3
                var response = await httpClient.PostAsync($"{controller}/uploadgroupfile?groupId={groupId}", content);
                if (response.IsSuccessStatusCode)
                {
                    string stringContent = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(stringContent))
                        return default;
                    return stringContent;
                }
                else
                    throw new SystemException("Gagal Upload File");
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }


        public async Task<string> UploadPrivateFile(string temainid, MultipartFormDataContent content)
        {
            try
            {
                var response = await httpClient.PostAsync($"{controller}/uploadprivatefile?temainid={temainid}", content);
                if (response.IsSuccessStatusCode)
                {
                    string stringContent = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(stringContent))
                        return default;
                    return stringContent;
                }
                else
                    throw new SystemException("Gagal Upload File");
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

    }



}
