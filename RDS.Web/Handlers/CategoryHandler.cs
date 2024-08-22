namespace RDS.Web.Handlers;

public class CategoryHandler(HttpClientService httpClientService) : ICategoryHandler
{
    private readonly Lazy<Task<HttpClient>> _httpClient = new(httpClientService.GetHttpClientAsync);

    private async Task<HttpClient> GetHttpClientAsync()
    {
        return await _httpClient.Value;
    }

    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/categories/create")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, 400, "Falha ao criar a categoria");
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"v1/categories/update")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, 400, "Falha ao atualizar a categoria");
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"v1/categories/delete")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, 400, "Falha ao excluir a categoria");
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/categories/byid")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, 400, "Não foi possível obter a categoria");
    }
    
    public async Task<PagedResponse<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/categories/all")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<PagedResponse<List<Category>>>()
               ?? new PagedResponse<List<Category>>(null, 400, "Não foi possível obter as categorias");
    }
}