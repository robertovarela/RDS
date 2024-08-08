namespace RDS.Web.Handlers;

public class ApplicationUserHandler(HttpClientService httpClientService) : IApplicationUserHandler
{
    private readonly Lazy<Task<HttpClient>> _httpClient = new(httpClientService.GetHttpClientAsync);
    private async Task<HttpClient> GetHttpClientAsync()
    {
        return await _httpClient.Value;
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

    public async Task<PagedResponse<List<ApplicationUser>>> GetAllAsync(GetAllApplicationUserRequest request)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Post, //$"v1/users/allusers") 
            $"v1/users/allusers?pageNumber={request.PageNumber}&pageSize={request.PageSize}")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<PagedResponse<List<ApplicationUser>>>()
               ?? new PagedResponse<List<ApplicationUser>>(null, 400, "Não foi possível obter os usuários");
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