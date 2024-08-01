using System.Net.Http.Json;
using RDS.Core.Handlers;
using RDS.Core.Models.ApplicationUser;
using RDS.Core.Requests.ApplicationUsers.Address;
using RDS.Core.Responses;
using RDS.Web.Services;

namespace RDS.Web.Handlers;

public class ApplicationUserAdressHandler(HttpClientService httpClientService, HttpClient httpClientJwt) : IApplicationUserAddressHandler
{
 private async Task EnsureHttpClientInitializedAsync()
    {
        if (httpClientJwt == null)
        {
            httpClientJwt = await httpClientService.GetHttpClientAsync();
        }
    }

    public async Task<Response<ApplicationUserAddress?>> CreateAsync(CreateApplicationUserAddressRequest request)
    {
        await EnsureHttpClientInitializedAsync();

        var result = await httpClientJwt.PostAsJsonAsync("v1/users/address/create", request);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUserAddress?>>()
               ?? new Response<ApplicationUserAddress?>(null, 400, "Falha ao criar o endereço");
    }

    public async Task<Response<ApplicationUserAddress?>> DeleteAsync(DeleteApplicationUserAddressRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"v1/users/address/delete")
        {
            Content = JsonContent.Create(request)
        };

        var result = await httpClientJwt.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUserAddress?>>()
               ?? new Response<ApplicationUserAddress?>(null, 400, "Falha ao excluir o endereço");
    }

    public async Task<Response<ApplicationUserAddress?>> DeleteAsyncOld(DeleteApplicationUserAddressRequest request)
    {
        var result = await httpClientJwt.DeleteAsync($"v1/users/address/delete");
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUserAddress?>>()
               ?? new Response<ApplicationUserAddress?>(null, 400, "Falha ao excluir o endereço");
    }

    public async Task<PagedResponse<List<ApplicationUserAddress>>> GetAllAsync(GetAllApplicationUserAddressRequest request)
    => await httpClientJwt.GetFromJsonAsync<PagedResponse<List<ApplicationUserAddress>>>($"v1/users/address/alladdresses")
           ?? new PagedResponse<List<ApplicationUserAddress>>(null, 400, "Não foi possível obter os endereços");

    public async Task<Response<ApplicationUserAddress?>> GetByIdAsync(GetApplicationUserAddressByIdRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/address/addressbyid")
        {
            Content = JsonContent.Create(request)
        };

        var result = await httpClientJwt.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUserAddress?>>()
               ?? new Response<ApplicationUserAddress?>(null, 400, "Falha ao obter o endereço");
    }

    public async Task<Response<ApplicationUserAddress?>> GetByIdAsyncOriginal(GetApplicationUserAddressByIdRequest request)
     => await httpClientJwt.GetFromJsonAsync<Response<ApplicationUserAddress?>>($"v1/users/address/addressbyid/{request.Id}")
           ?? new Response<ApplicationUserAddress?>(null, 400, "Não foi possível obter o endereço");

    public async Task<Response<ApplicationUserAddress?>> UpdateAsync(UpdateApplicationUserAddressRequest request)
    {
        var result = await httpClientJwt.PutAsJsonAsync($"v1/users/address/update", request);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUserAddress?>>()
               ?? new Response<ApplicationUserAddress?>(null, 400, "Falha ao atualizar o endereço");
    }
}