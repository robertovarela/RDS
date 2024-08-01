namespace RDS.Web.Services;

using System.Net.Http.Headers;
using Blazored.LocalStorage;

// public class HttpClientService(HttpClient httpClient, ILocalStorageService localStorageService)
// {
//     private bool _isInitialized;
//
//     public async Task<HttpClient> GetHttpClientAsync()
//     {
//         if (_isInitialized) return httpClient;
//
//         var token = await localStorageService.GetItemAsync<string>("authToken");
//
//         httpClient.DefaultRequestHeaders.Authorization = !string.IsNullOrEmpty(token) ? 
//             new AuthenticationHeaderValue("Bearer", token) : null;
//
//         _isInitialized = true;
//
//         return httpClient;
//     }
// }

public class HttpClientService(HttpClient httpClient, ILocalStorageService localStorageService)
{
    public async Task<HttpClient> GetHttpClientAsync()
    {
        var token = await localStorageService.GetItemAsync<string>("authToken");
        if (!string.IsNullOrEmpty(token))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            httpClient.DefaultRequestHeaders.Authorization = null;
        }

        return httpClient;
    }
}