using Blazored.LocalStorage;
using System.Net.Http;

namespace MarampaApp.Client.OcphAuthClient
{
    public class HttpClientAuthenticationHandler : DelegatingHandler
    {
        public ILocalStorageService LocalStorageService { get; }

        public HttpClientAuthenticationHandler(ILocalStorageService localStorageService)
        {
            LocalStorageService = localStorageService;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await LocalStorageService.GetItemAsync<string>("token");
            if (!string.IsNullOrEmpty(token))
            {
               request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return await base.SendAsync(request, cancellationToken);
        }
        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = LocalStorageService.GetItemAsync<string>("token").Result;
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return base.Send(request, cancellationToken);
        }

    }
}
