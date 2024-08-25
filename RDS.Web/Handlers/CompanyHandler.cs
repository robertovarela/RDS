namespace RDS.Web.Handlers;

public class CompanyHandler(HttpClientService httpClientService) : ICompanyHandler
{
    private readonly Lazy<Task<HttpClient>> _httpClient = new(httpClientService.GetHttpClientAsync);

    private async Task<HttpClient> GetHttpClientAsync()
    {
        return await _httpClient.Value;
    }

    public async Task<Response<Company?>> CreateAsync(CreateCompanyRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/companies/create")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<Company?>>()
               ?? new Response<Company?>(null, 400, "Falha ao criar a empresa");    
    }

    public async Task<Response<Company?>> UpdateAsync(UpdateCompanyRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"v1/companies/update")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<Company?>>()
               ?? new Response<Company?>(null, 400, "Falha ao atualizar a empresa");    
    }

    public async Task<Response<Company?>> DeleteAsync(DeleteCompanyRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"v1/companies/delete")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<Company?>>()
               ?? new Response<Company?>(null, 400, "Falha ao excluir a empresa");    
    }

    public async Task<Response<List<Company>>> GetAllAsync(GetAllCompaniesRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/companies/all")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<PagedResponse<List<Company>>>()
               ?? new PagedResponse<List<Company>>(null, 400, "Não foi possível obter as empresas");    
    }
  
    public async Task<Response<List<Company>>> GetAllByUserIdAsync(GetAllCompaniesByUserIdRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/companies/allbyuserid")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<PagedResponse<List<Company>>>()
               ?? new PagedResponse<List<Company>>(null, 400, "Não foi possível obter as empresas");    
    }
    
    public async Task<PagedResponse<List<AllCompaniesIdViewModel>>> GetAllCompanyIdByUserIdAsync(GetAllCompaniesByUserIdRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/companies/allcompanyidbyuserid")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<PagedResponse<List<AllCompaniesIdViewModel>>>()
               ?? new PagedResponse<List<AllCompaniesIdViewModel>>(null, 400, "Não foi possível obter as empresas");    
    }
    
    public async Task<Response<Company?>> GetByIdAsync(GetCompanyByIdRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/companies/byid")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<Company?>>()
               ?? new Response<Company?>(null, 400, "Não foi possível obter a empresa");    
    }
}