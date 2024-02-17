using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatAppMobile.Services
{
    public interface IFileServices
    {
        Task DownloadFile(Message message);
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

        public async Task DownloadFile(Message message)
        {
            try
            {
                var cancellationToken = new CancellationToken();
                ///api / Files / uploadgroupfile ? groupid = 3
                var response = await httpClient.GetByteArrayAsync($"{controller}/getbykey?key={message.UrlFile.Substring(1,message.UrlFile.Length-1)}");
                using var stream = new MemoryStream(response);
                var fileSaverResult = await FileSaver.Default.SaveAsync(message.Text, stream, cancellationToken);
                if (fileSaverResult.IsSuccessful)
                {
                    await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show(cancellationToken);
                }
                else
                {
                    await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<string> UploadGroupFile(int groupId, MultipartFormDataContent content)
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
