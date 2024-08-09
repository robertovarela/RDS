namespace RDS.Web.Services;

public class AuthenticationService(
    HttpClient httpClient,
    AuthenticationStateProvider authenticationStateProvider,
    ILocalStorageService localStorageService,
    ILogger<AuthenticationService> logger)
{
    public async Task<bool> LoginAsync(LoginRequest request)
    {
        logger.LogInformation("Starting LoginAsync");

        try
        {
            var response = await httpClient.PostAsJsonAsync("v1/users/userlogin", request);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                logger.LogInformation($"Response Content: {responseContent}");
   
                var result = await response.Content.ReadFromJsonAsync<Response<UserLogin>>();
                var token = result?.Data?.Token;

                if (string.IsNullOrEmpty(token))
                {
                    logger.LogError("Token is null or empty");

                    return false;
                }

                await localStorageService.SetItemAsync("authToken", token);
                ((CustomAuthenticationStateProvider)authenticationStateProvider).NotifyUserAuthentication(token);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return true;
            }
            else
            {
                logger.LogError($"Login failed with status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred during login");
        }

        return false;
    }

    public async Task LogoutAsync()
    {
        await localStorageService.RemoveItemAsync("authToken");
        ((CustomAuthenticationStateProvider)authenticationStateProvider).NotifyUserLogout();
        httpClient.DefaultRequestHeaders.Authorization = null;
    }
    
    public async Task<bool> RefreshTokenAsync(RefreshTokenRequest request)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("v1/users/refreshtoken", request);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                logger.LogInformation($"Response Content: {responseContent}");
   
                var result = await response.Content.ReadFromJsonAsync<Response<UserLogin>>();
                var token = result?.Data?.Token;

                if (string.IsNullOrEmpty(token))
                {
                    return false;
                }

                await localStorageService.SetItemAsync("authToken", token);
                ((CustomAuthenticationStateProvider)authenticationStateProvider).NotifyUserAuthentication(token);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return true;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred during login");
        }

        return false;
    }
}