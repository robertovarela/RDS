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
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/create")
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
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"v1/useridentity/updateuser")
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
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, "v1/useridentity/deleteuser")
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
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/useridentity/allusers")
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
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/useridentity/userbyid")
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
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/useridentity/userbycpf")
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
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/useridentity/userbyname")
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
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/useridentity/userbyfullname")
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


// namespace RDS.Web.Handlers;
//
// public class ApplicationUserHandler(HttpClientService httpClientService, HttpClient httpClientJwt) : IApplicationUserHandler
// {
//     private async Task EnsureHttpClientInitializedAsync()
//     {
//         if (httpClientJwt == null)
//         {
//             httpClientJwt = await httpClientService.GetHttpClientAsync();
//         }
//     }
//
//     public async Task<Response<ApplicationUser?>> CreateAsync(CreateApplicationUserRequest request)
//     {
//         await EnsureHttpClientInitializedAsync();
//         var result = await httpClientJwt.PostAsJsonAsync("v1/users/create", request);
//         return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
//                ?? new Response<ApplicationUser?>(null, 400, "Falha ao criar o usuário");
//     }
//     
//     public async Task<Response<ApplicationUser?>> UpdateAsync(UpdateApplicationUserRequest request)
//     {
//         await EnsureHttpClientInitializedAsync();
//         var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"v1/useridentity/updateuser")
//         {
//             Content = JsonContent.Create(request)
//         };
//         var result = await httpClientJwt.SendAsync(requestMessage);
//         return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
//                ?? new Response<ApplicationUser?>(null, 400, "Falha ao atualizar o usuário");
//     }
//     
//     public async Task<Response<ApplicationUser?>> DeleteAsync(DeleteApplicationUserRequest request)
//     {
//         await EnsureHttpClientInitializedAsync();
//         var requestMessage = new HttpRequestMessage(HttpMethod.Delete, "v1/useridentity/deleteuser")
//         {
//             Content = JsonContent.Create(request)
//         };
//         var result = await httpClientJwt.SendAsync(requestMessage);
//         return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
//                ?? new Response<ApplicationUser?>(null, 400, "Falha ao excluir o usuário");
//     }
//
//     public async Task<PagedResponse<List<ApplicationUser>>> GetAllAsync(GetAllApplicationUserRequest request)
//     {
//         await EnsureHttpClientInitializedAsync();
//         return await httpClientJwt.GetFromJsonAsync<PagedResponse<List<ApplicationUser>>>("v1/useridentity/allusers")
//                ?? new PagedResponse<List<ApplicationUser>>(null, 400, "Não foi possível obter os usuários");
//     }
//
//     public async Task<Response<ApplicationUser?>> GetByIdAsync(GetApplicationUserByIdRequest request)
//     {
//         await EnsureHttpClientInitializedAsync();
//         var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/useridentity/userbyid")
//         {
//             Content = JsonContent.Create(request)
//         };
//         var result = await httpClientJwt.SendAsync(requestMessage);
//         return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
//                ?? new Response<ApplicationUser?>(null, 400, "Não foi possível obter o usuário");
//     }
//
//     public async Task<Response<ApplicationUser?>> GetByCpfAsync(GetApplicationUserByCpfRequest request)
//     {
//         await EnsureHttpClientInitializedAsync();
//         var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/useridentity/userbycpf")
//         {
//             Content = JsonContent.Create(request)
//         };
//         var result = await httpClientJwt.SendAsync(requestMessage);
//         return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
//                ?? new Response<ApplicationUser?>(null, 400, "Não foi possível obter o usuário");
//     }
//
//     public async Task<PagedResponse<List<ApplicationUser>>> GetByNameAsync(GetApplicationUserByNameRequest request)
//     {
//         await EnsureHttpClientInitializedAsync();
//         var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/useridentity/userbyname")
//         {
//             Content = JsonContent.Create(request)
//         };
//         var result = await httpClientJwt.SendAsync(requestMessage);
//         return await result.Content.ReadFromJsonAsync<PagedResponse<List<ApplicationUser>>?>()
//                ?? new PagedResponse<List<ApplicationUser>>(null, 400, "Não foi possível obter o usuário");
//     }
//     
//     public async Task<PagedResponse<List<ApplicationUser>>> GetByFullNameAsync(GetApplicationUserByFullNameRequest request)
//     {
//         await EnsureHttpClientInitializedAsync();
//         var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/useridentity/userbyfullname")
//         {
//             Content = JsonContent.Create(request)
//         };
//         var result = await httpClientJwt.SendAsync(requestMessage);
//         return await result.Content.ReadFromJsonAsync<PagedResponse<List<ApplicationUser>>?>()
//                ?? new PagedResponse<List<ApplicationUser>>(null, 400, "Não foi possível obter o usuário");
//     }
//     
//     public async Task<Response<ApplicationUser?>> GetActiveAsync(GetApplicationUserActiveRequest request)
//     {
//         await EnsureHttpClientInitializedAsync();
//         return await httpClientJwt.GetFromJsonAsync<Response<ApplicationUser?>>($"v1/users/useractive")
//                ?? new Response<ApplicationUser?>(null, 400, "Não foi possível obter o usuário logado");
//     }
// }