﻿namespace RDS.Web.Handlers;

public class ApplicationUserConfigurationHandler(HttpClientService httpClientService)
    : IApplicationUserConfigurationHandler
{
    private readonly Lazy<Task<HttpClient>> _httpClient = new(httpClientService.GetHttpClientAsync);

    private async Task<HttpClient> GetHttpClientAsync()
    {
        return await _httpClient.Value;
    }

    public async Task<Response<ApplicationRole?>> CreateRoleAsync(CreateApplicationRoleRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/userconfiguration/create-role")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationRole?>>()
               ?? new Response<ApplicationRole?>(null, 400, "Falha ao criar a role");
    }

    public async Task<Response<ApplicationRole?>> DeleteRoleAsync(DeleteApplicationRoleRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"v1/userconfiguration/delete-role")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationRole?>>()
               ?? new Response<ApplicationRole?>(null, 400, "Falha ao excluir a role");
    }

    public async Task<Response<ApplicationUserRole?>> CreateUserRoleAsync(CreateApplicationUserRoleRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/userconfiguration/create-role-to-user")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUserRole?>>()
               ?? new Response<ApplicationUserRole?>(null, 400, "Falha ao criar a role do usuário");
    }

    public async Task<Response<ApplicationUserRole?>> DeleteUserRoleAsync(DeleteApplicationUserRoleRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"v1/userconfiguration/delete-role-to-user")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUserRole?>>()
               ?? new Response<ApplicationUserRole?>(null, 400, "Falha ao excluir a role do usuário");
    }
}