namespace RDS.Web.Handlers;

public class ApplicationUserAddressHandler(HttpClientService httpClientService) : IApplicationUserAddressHandler
{
    private readonly Lazy<Task<HttpClient>> _httpClient = new(httpClientService.GetHttpClientAsync);
    private async Task<HttpClient> GetHttpClientAsync()
    {
        return await _httpClient.Value;
    }

    public async Task<Response<ApplicationUserAddress?>> CreateAsync(CreateApplicationUserAddressRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/addresses/createuseraddress")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUserAddress?>>()
               ?? new Response<ApplicationUserAddress?>(null, 400, "Falha ao criar o endereço");
    }
    
    public async Task<Response<ApplicationUserAddress?>> UpdateAsync(UpdateApplicationUserAddressRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"v1/users/addresses/updateuseraddress")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUserAddress?>>()
               ?? new Response<ApplicationUserAddress?>(null, 400, "Falha ao atualizar o usuário");
    }

    public async Task<Response<ApplicationUserAddress?>> DeleteAsync(DeleteApplicationUserAddressRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, "v1/users/addresses/deleteuseraddress")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUserAddress?>>()
               ?? new Response<ApplicationUserAddress?>(null, 400, "Falha ao excluir o endereço");
    }

    public async Task<PagedResponse<List<ApplicationUserAddress>>> GetAllAsync(GetAllApplicationUserAddressRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/addresses/alluseraddresses")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<PagedResponse<List<ApplicationUserAddress>>>()
               ?? new PagedResponse<List<ApplicationUserAddress>>(null, 400, "Não foi possível obter os endereços");
    }

    public async Task<Response<ApplicationUserAddress?>> GetByIdAsync(GetApplicationUserAddressByIdRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/addresses/useraddressbyid")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUserAddress?>>()
               ?? new Response<ApplicationUserAddress?>(null, 400, "Não foi possível obter o usuário");
    }
}