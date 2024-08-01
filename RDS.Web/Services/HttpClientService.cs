namespace RDS.Web.Services;

public class HttpClientService(HttpClient httpClient, ILocalStorageService localStorageService)
{
    private bool _isInitialized;

    public async Task<HttpClient> GetHttpClientAsync()
    {
        if (_isInitialized) return httpClient;

        var token = await localStorageService.GetItemAsync<string>("authToken");

        httpClient.DefaultRequestHeaders.Authorization = !string.IsNullOrEmpty(token) ? 
            new AuthenticationHeaderValue("Bearer", token) : null;

        _isInitialized = true;

        return httpClient;
    }
}