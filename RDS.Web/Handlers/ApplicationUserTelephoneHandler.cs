namespace RDS.Web.Handlers;

public class ApplicationUserTelephoneHandler(HttpClientService httpClientService) : IApplicationUserTelephoneHandler
{
    private readonly Lazy<Task<HttpClient>> _httpClient = new(httpClientService.GetHttpClientAsync);

    private async Task<HttpClient> GetHttpClientAsync()
    {
        return await _httpClient.Value;
    }

    public async Task<Response<ApplicationUserTelephone?>> CreateAsync(CreateApplicationUserTelephoneRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/telephones/createusertelephone")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUserTelephone?>>()
               ?? new Response<ApplicationUserTelephone?>(null, 400, "Falha ao criar o telefone");
    }

    public async Task<Response<ApplicationUserTelephone?>> UpdateAsync(UpdateApplicationUserTelephoneRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"v1/users/telephones/updateusertelephone")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUserTelephone?>>()
               ?? new Response<ApplicationUserTelephone?>(null, 400, "Falha ao atualizar o telefone");
    }

    public async Task<Response<ApplicationUserTelephone?>> DeleteAsync(DeleteApplicationUserTelephoneRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, "v1/users/telephones/deleteusertelephone")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUserTelephone?>>()
               ?? new Response<ApplicationUserTelephone?>(null, 400, "Falha ao excluir o telefone");
    }

    public async Task<PagedResponse<List<ApplicationUserTelephone>>> GetAllAsync(
        GetAllApplicationUserTelephoneRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/telephones/allusertelephones")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<PagedResponse<List<ApplicationUserTelephone>>>()
               ?? new PagedResponse<List<ApplicationUserTelephone>>(null, 400, "Não foi possível obter os telefones");
    }

    public async Task<Response<ApplicationUserTelephone?>> GetByIdAsync(GetApplicationUserTelephoneByIdRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/users/telephones/usertelephonebyid")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<ApplicationUserTelephone?>>()
               ?? new Response<ApplicationUserTelephone?>(null, 400, "Não foi possível obter o telefone");
    }
}