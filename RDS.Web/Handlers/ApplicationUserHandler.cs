namespace RDS.Web.Handlers;

public class ApplicationUserHandler(
    HttpClientService httpClientService,
    AuthenticationStateProvider authenticationStateProvider,
    ILocalStorageService localStorageService,
    ILogger<AuthenticationService> logger) : IApplicationUserHandler
{
    private readonly Lazy<Task<HttpClient>> _httpClient = new(httpClientService.GetHttpClientAsync);

    private async Task<HttpClient> GetHttpClientAsync()
    {
        return await _httpClient.Value;
    }

    public async Task<Response<UserLogin>> LoginAsync(LoginRequest? request)
    {
        logger.LogInformation("Starting LoginAsync");

        try
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/userlogin")
            {
                Content = JsonContent.Create(request)
            };
            var httpClient = await GetHttpClientAsync();
            var response = await httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                logger.LogInformation($"Response Content: {responseContent}");

                var result = await response.Content.ReadFromJsonAsync<Response<UserLogin>>();
                var token = result?.Data?.Token;

                if (string.IsNullOrEmpty(token))
                {
                    logger.LogError("Token is null or empty");

                    return new Response<UserLogin>(null, 400, "Falha ao criar o usuário");
                }

                await localStorageService.SetItemAsync("authToken", token);
                ((CustomAuthenticationStateProvider)authenticationStateProvider).NotifyUserAuthentication(token);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return new Response<UserLogin>(null, 200, "Success");
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

        return new Response<UserLogin>(null, 400, "Falha ao criar o usuário");
    }

    public async Task<Response<UserRefreshToken>> RefreshTokenAsync(RefreshTokenRequest? request)
    {
        try
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/refreshtoken")
            {
                Content = JsonContent.Create(request)
            };
            var httpClient = await GetHttpClientAsync();
            var response = await httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                //var responseContent = await response.Content.ReadAsStringAsync();

                var result = await response.Content.ReadFromJsonAsync<Response<UserRefreshToken>>();
                var token = result?.Data?.Token;

                if (string.IsNullOrEmpty(token))
                {
                    return new Response<UserRefreshToken>(null, 400, "Falha ao renovar o token");
                }

                await localStorageService.SetItemAsync("authToken", token);
                ((CustomAuthenticationStateProvider)authenticationStateProvider).NotifyUserAuthentication(token);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return new Response<UserRefreshToken>(null, 200, "Success");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred during token refresh");
        }

        return new Response<UserRefreshToken>(null, 400, "Falha ao renovar o token");
    }

    public async Task<Response<ApplicationUser?>> CreateAsync(CreateApplicationUserRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/createuser")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
               ?? new Response<ApplicationUser?>(null, 400, "Falha ao criar o usuário");
    }

    public async Task<Response<ApplicationUser?>> UpdateAsync(UpdateApplicationUserRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"v1/users/updateuser")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
               ?? new Response<ApplicationUser?>(null, 400, "Falha ao atualizar o usuário");
    }

    public async Task<Response<ApplicationUser?>> DeleteAsync(DeleteApplicationUserRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, "v1/users/deleteuser")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
               ?? new Response<ApplicationUser?>(null, 400, "Falha ao excluir o usuário");
    }

    public async Task<PagedResponse<List<AllUsersViewModel>>> GetAllAsync(GetAllApplicationUserRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/allusers")
            {
                Content = JsonContent.Create(request)
            };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<PagedResponse<List<AllUsersViewModel>>>()
               ?? new PagedResponse<List<AllUsersViewModel>>(null, 400, "Não foi possível obter os usuários");
    }

    public async Task<PagedResponse<List<AllUsersViewModel>>> GetAllByCompanyIdAsync(GetAllApplicationUserRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/allusersbycompanyid")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<PagedResponse<List<AllUsersViewModel>>>()
               ?? new PagedResponse<List<AllUsersViewModel>>(null, 400, "Não foi possível obter os usuários");
    }

    public async Task<Response<ApplicationUser?>> GetByIdAsync(GetApplicationUserByIdRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/userbyid")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
               ?? new Response<ApplicationUser?>(null, 400, "Não foi possível obter o usuário");
    }

    public async Task<Response<ApplicationUser?>> GetByCpfAsync(GetApplicationUserByCpfRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/v/userbycpf")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
               ?? new Response<ApplicationUser?>(null, 400, "Não foi possível obter o usuário");
    }

    public async Task<PagedResponse<List<ApplicationUser>>> GetByNameAsync(GetApplicationUserByNameRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/userbyname")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<PagedResponse<List<ApplicationUser>>?>()
               ?? new PagedResponse<List<ApplicationUser>>(null, 400, "Não foi possível obter o usuário");
    }

    public async Task<PagedResponse<List<ApplicationUser>>> GetByFullNameAsync(
        GetApplicationUserByFullNameRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/userbyfullname")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<PagedResponse<List<ApplicationUser>>?>()
               ?? new PagedResponse<List<ApplicationUser>>(null, 400, "Não foi possível obter o usuário");
    }

    public async Task<Response<ApplicationUser?>> GetActiveAsync(GetApplicationUserActiveRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/useractive")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
               ?? new Response<ApplicationUser?>(null, 400, "Não foi possível obter o usuário logado");
    }
}