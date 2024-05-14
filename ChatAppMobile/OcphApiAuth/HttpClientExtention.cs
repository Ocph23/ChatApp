using ChatAppMobile;
using Client;
using MarampaApp;
using Shared;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace OcphApiAuth.Client
{
    public static class HttpClientExtention
    {
        public static async Task<T> GetResultAsync<T>(this HttpResponseMessage response)
        {
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    ArgumentNullException.ThrowIfNull(response);
                    if (response.StatusCode == HttpStatusCode.NoContent)
                        throw new ArgumentNullException("Data Tidak Ditemukan");

                    string? stringContent = await response.Content.ReadAsStringAsync();
                    ArgumentNullException.ThrowIfNullOrEmpty(stringContent);
                    var result = JsonSerializer.Deserialize<T>(stringContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    ArgumentNullException.ThrowIfNull(result);
                    return result;
                }
                throw new Exception("Maaf Terjadi Kesalahan !");
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static async Task SetToken(this HttpClient httpClient)
        {
            try
            {
                var token = Preferences.Get("token", string.Empty);
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            catch (Exception)
            {
            }
        }

        public static StringContent GenerateHttpContent(this HttpClient client, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }


        public static async Task<string> Error(this HttpResponseMessage response)
        {
            try
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return $"'{response.RequestMessage.RequestUri.LocalPath}'  Not Found";

                if (response.StatusCode == System.Net.HttpStatusCode.MethodNotAllowed)
                    return $"'{response.RequestMessage.RequestUri.LocalPath}'  Method Not Allowed";

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    
                    return $"Not Have Access !";
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                    throw new SystemException("Maaf terjadi kesalahan, Coba ulangi lagi");

                if (content.Contains("message"))
                {
                    var error = JsonSerializer.Deserialize<ErrorMessage>(content, Helper.JsonOptions);
                    return error.Message!;
                }
                else if (content.Contains("tools.ietf"))
                {
                    var errors = JsonSerializer.Deserialize<ErrorMessages>(content, Helper.JsonOptions);
                    return errors.Title!;
                }
                return content;
            }
            catch (Exception)
            {
                return "Upssss Sory ! Try Again.";
            }
        }
    }

    public class ErrorMessage
    {
        public string? Message { get; set; }
    }

    public class Errors
    {
        public List<string>? Email { get; set; }
    }

    public class ErrorMessages
    {
        public string? Type { get; set; }
        public string? Title { get; set; }
        public int Status { get; set; }
        public string? TraceId { get; set; }
        public Errors? Errors { get; set; }
    }

}
