// using System.Net.Http.Json;
// using RDS.Core.Handlers;
// using RDS.Core.Models.ApplicationUser;
// using RDS.Core.Requests.ApplicationUser;
// using RDS.Core.Requests.ApplicationUsers;
// using RDS.Core.Responses;
// using RDS.Web.Services;
//
// namespace RDS.Web.Handlers;
//
// public class ApplicationUserHandler(HttpClientService httpClientService) : IApplicationUserHandler
// {
//     private readonly HttpClient _httpClient = httpClientService.GetHttpClientAsync().Result;
//
//     public async Task<Response<ApplicationUser?>> CreateAsync(CreateApplicationUserRequest request)
//     {
//         var result = await _httpClient.PostAsJsonAsync("v1/users/create", request);
//         return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
//                ?? new Response<ApplicationUser?>(null, 400, "Falha ao criar o usuário");
//     }
//
//     public async Task<Response<ApplicationUser?>> UpdateAsync(UpdateApplicationUserRequest request)
//     {
//         var result = await _httpClient.PutAsJsonAsync($"v1/users/update", request);
//         return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
//                ?? new Response<ApplicationUser?>(null, 400, "Falha ao atualizar o usuário");
//     }
//     
//     public async Task<Response<ApplicationUser?>> DeleteAsync(DeleteApplicationUserRequest request)
//     {
//         var requestMessage = new HttpRequestMessage(HttpMethod.Delete, "v1/users/delete")
//         {
//             Content = JsonContent.Create(request)
//         };
//         var result = await _httpClient.SendAsync(requestMessage);
//         return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
//                ?? new Response<ApplicationUser?>(null, 400, "Falha ao excluir o usuário");
//     }
//
//     public async Task<PagedResponse<List<ApplicationUser>>> GetAllAsync(GetAllApplicationUserRequest request)
//     {
//         return await _httpClient.GetFromJsonAsync<PagedResponse<List<ApplicationUser>>>("v1/users/allusers")
//                ?? new PagedResponse<List<ApplicationUser>>(null, 400, "Não foi possível obter os usuários");
//     }
//
//     public async Task<Response<ApplicationUser?>> GetByIdAsync(GetApplicationUserByIdRequest request)
//     {
//         var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/userbyid")
//         {
//             Content = JsonContent.Create(request)
//         };
//         var result = await _httpClient.SendAsync(requestMessage);
//         return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
//                ?? new Response<ApplicationUser?>(null, 400, "Falha ao obter o usuário");
//     }
//     
//     public async Task<Response<ApplicationUser?>> GetByCpfAsync(GetApplicationUserByCpfRequest request)
//     {
//         return await _httpClient.GetFromJsonAsync<Response<ApplicationUser?>>($"v1/users/userbycpf/{request.Cpf}")
//                ?? new Response<ApplicationUser?>(null, 400, "Não foi possível obter o usuário");
//     }
//     
//     public async Task<PagedResponse<List<ApplicationUser>>> GetByNameAsync(GetApplicationUserByNameRequest request)
//     {
//         return await _httpClient.GetFromJsonAsync<PagedResponse<List<ApplicationUser>>>(
//                    $"v1/users/userbyname/{request.UserName}")
//                ?? new PagedResponse<List<ApplicationUser>>(null, 400, "Não foi possível obter o usuário");
//     }
//     
//     public async Task<PagedResponse<List<ApplicationUser>>> GetByFullNameAsync(
//         GetApplicationUserByFullNameRequest request)
//     {
//         return await _httpClient.GetFromJsonAsync<PagedResponse<List<ApplicationUser>>>(
//                    $"v1/users/userbyfullname/{request.UserName}")
//                ?? new PagedResponse<List<ApplicationUser>>(null, 400, "Não foi possível obter o usuário");
//     }
//
//     public async Task<Response<ApplicationUser?>> GetActiveAsync(GetApplicationUserActiveRequest request)
//     {
//         return await _httpClient.GetFromJsonAsync<Response<ApplicationUser?>>($"v1/users/useractive")
//                ?? new Response<ApplicationUser?>(null, 400, "Não foi possível obter o usuário logado");
//     }
// }


// using System.Net.Http.Json;
// using RDS.Core.Handlers;
// using RDS.Core.Models.ApplicationUser;
// using RDS.Core.Requests.ApplicationUser;
// using RDS.Core.Requests.ApplicationUsers;
// using RDS.Core.Responses;
// using RDS.Web.Services;
//
// namespace RDS.Web.Handlers;
//
// public class ApplicationUserHandler(HttpClientService httpClientService) : IApplicationUserHandler
// {
//     private async Task<HttpClient> GetHttpClientAsync()
//     {
//         return await httpClientService.GetHttpClientAsync();
//     }
//
//     public async Task<Response<ApplicationUser?>> CreateAsync(CreateApplicationUserRequest request)
//     {
//         var httpClient = await GetHttpClientAsync();
//         var result = await httpClient.PostAsJsonAsync("v1/users/create", request);
//         return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
//                ?? new Response<ApplicationUser?>(null, 400, "Falha ao criar o usuário");
//     }
//
//     public async Task<Response<ApplicationUser?>> DeleteAsync(DeleteApplicationUserRequest request)
//     {
//         var httpClient = await GetHttpClientAsync();
//         var requestMessage = new HttpRequestMessage(HttpMethod.Delete, "v1/users/delete")
//         {
//             Content = JsonContent.Create(request)
//         };
//         var result = await httpClient.SendAsync(requestMessage);
//         return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
//                ?? new Response<ApplicationUser?>(null, 400, "Falha ao excluir o usuário");
//     }
//
//     public async Task<PagedResponse<List<ApplicationUser>>> GetAllAsync(GetAllApplicationUserRequest request)
//     {
//         var httpClient = await GetHttpClientAsync();
//         return await httpClient.GetFromJsonAsync<PagedResponse<List<ApplicationUser>>>("v1/users/allusers")
//                ?? new PagedResponse<List<ApplicationUser>>(null, 400, "Não foi possível obter os usuários");
//     }
//
//     public async Task<PagedResponse<List<ApplicationUser>>> GetByFullNameAsync(
//         GetApplicationUserByFullNameRequest request)
//     {
//         var httpClient = await GetHttpClientAsync();
//         return await httpClient.GetFromJsonAsync<PagedResponse<List<ApplicationUser>>>(
//                    $"v1/users/userbyfullname/{request.UserName}")
//                ?? new PagedResponse<List<ApplicationUser>>(null, 400, "Não foi possível obter o usuário");
//     }
//
//     public async Task<Response<ApplicationUser?>> GetByCpfAsync(GetApplicationUserByCpfRequest request)
//     {
//         var httpClient = await GetHttpClientAsync();
//         return await httpClient.GetFromJsonAsync<Response<ApplicationUser?>>($"v1/users/userbycpf/{request.Cpf}")
//                ?? new Response<ApplicationUser?>(null, 400, "Não foi possível obter o usuário");
//     }
//
//     public async Task<Response<ApplicationUser?>> GetByIdAsync(GetApplicationUserByIdRequest request)
//     {
//         var httpClient = await GetHttpClientAsync();
//         var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/userbyid")
//         {
//             Content = JsonContent.Create(request)
//         };
//         var result = await httpClient.SendAsync(requestMessage);
//         return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
//                ?? new Response<ApplicationUser?>(null, 400, "Falha ao obter o usuário");
//     }
//
//     public async Task<Response<ApplicationUser?>> GetActiveAsync(GetApplicationUserActiveRequest request)
//     {
//         var httpClient = await GetHttpClientAsync();
//         return await httpClient.GetFromJsonAsync<Response<ApplicationUser?>>($"v1/users/useractive")
//                ?? new Response<ApplicationUser?>(null, 400, "Não foi possível obter o usuário logado");
//     }
//
//     public async Task<PagedResponse<List<ApplicationUser>>> GetByNameAsync(GetApplicationUserByNameRequest request)
//     {
//         var httpClient = await GetHttpClientAsync();
//         return await httpClient.GetFromJsonAsync<PagedResponse<List<ApplicationUser>>>(
//                    $"v1/users/userbyname/{request.UserName}")
//                ?? new PagedResponse<List<ApplicationUser>>(null, 400, "Não foi possível obter o usuário");
//     }
//
//     public async Task<Response<ApplicationUser?>> UpdateAsync(UpdateApplicationUserRequest request)
//     {
//         var httpClient = await GetHttpClientAsync();
//         var result = await httpClient.PutAsJsonAsync($"v1/users/update", request);
//         return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
//                ?? new Response<ApplicationUser?>(null, 400, "Falha ao atualizar o usuário");
//     }
// }


namespace RDS.Web.Handlers;

public class ApplicationUserHandler(HttpClientService httpClientService, HttpClient httpClientJwt) : IApplicationUserHandler
{
    private async Task EnsureHttpClientInitializedAsync()
    {
        if (httpClientJwt == null)
        {
            httpClientJwt = await httpClientService.GetHttpClientAsync();
        }
    }

    public async Task<Response<ApplicationUser?>> CreateAsync(CreateApplicationUserRequest request)
    {
        await EnsureHttpClientInitializedAsync();
        var result = await httpClientJwt.PostAsJsonAsync("v1/users/create", request);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
               ?? new Response<ApplicationUser?>(null, 400, "Falha ao criar o usuário");
    }
    
    public async Task<Response<ApplicationUser?>> UpdateAsync(UpdateApplicationUserRequest request)
    {
        await EnsureHttpClientInitializedAsync();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"v1/useridentity/updateuser")
        {
            Content = JsonContent.Create(request)
        };
        var result = await httpClientJwt.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
               ?? new Response<ApplicationUser?>(null, 400, "Falha ao atualizar o usuário");
    }
    
    public async Task<Response<ApplicationUser?>> DeleteAsync(DeleteApplicationUserRequest request)
    {
        await EnsureHttpClientInitializedAsync();
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, "v1/useridentity/deleteuser")
        {
            Content = JsonContent.Create(request)
        };
        var result = await httpClientJwt.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
               ?? new Response<ApplicationUser?>(null, 400, "Falha ao excluir o usuário");
    }

    public async Task<PagedResponse<List<ApplicationUser>>> GetAllAsync(GetAllApplicationUserRequest request)
    {
        await EnsureHttpClientInitializedAsync();
        return await httpClientJwt.GetFromJsonAsync<PagedResponse<List<ApplicationUser>>>("v1/useridentity/allusers")
               ?? new PagedResponse<List<ApplicationUser>>(null, 400, "Não foi possível obter os usuários");
    }

    public async Task<Response<ApplicationUser?>> GetByIdAsync(GetApplicationUserByIdRequest request)
    {
        await EnsureHttpClientInitializedAsync();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/useridentity/userbyid")
        {
            Content = JsonContent.Create(request)
        };
        var result = await httpClientJwt.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
               ?? new Response<ApplicationUser?>(null, 400, "Não foi possível obter o usuário");
    }

    public async Task<Response<ApplicationUser?>> GetByCpfAsync(GetApplicationUserByCpfRequest request)
    {
        await EnsureHttpClientInitializedAsync();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/useridentity/userbycpf")
        {
            Content = JsonContent.Create(request)
        };
        var result = await httpClientJwt.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUser?>>()
               ?? new Response<ApplicationUser?>(null, 400, "Não foi possível obter o usuário");
    }

    public async Task<PagedResponse<List<ApplicationUser>>> GetByNameAsync(GetApplicationUserByNameRequest request)
    {
        await EnsureHttpClientInitializedAsync();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/useridentity/userbyname")
        {
            Content = JsonContent.Create(request)
        };
        var result = await httpClientJwt.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<PagedResponse<List<ApplicationUser>>?>()
               ?? new PagedResponse<List<ApplicationUser>>(null, 400, "Não foi possível obter o usuário");
    }
    
    public async Task<PagedResponse<List<ApplicationUser>>> GetByFullNameAsync(GetApplicationUserByFullNameRequest request)
    {
        await EnsureHttpClientInitializedAsync();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/useridentity/userbyfullname")
        {
            Content = JsonContent.Create(request)
        };
        var result = await httpClientJwt.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<PagedResponse<List<ApplicationUser>>?>()
               ?? new PagedResponse<List<ApplicationUser>>(null, 400, "Não foi possível obter o usuário");
    }
    
    public async Task<Response<ApplicationUser?>> GetActiveAsync(GetApplicationUserActiveRequest request)
    {
        await EnsureHttpClientInitializedAsync();
        return await httpClientJwt.GetFromJsonAsync<Response<ApplicationUser?>>($"v1/users/useractive")
               ?? new Response<ApplicationUser?>(null, 400, "Não foi possível obter o usuário logado");
    }
}