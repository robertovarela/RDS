using System.Net.Http.Json;
using RDS.Core.Common.Extensions;
using RDS.Core.Handlers;
using RDS.Core.Models;
using RDS.Core.Requests.Transactions;
using RDS.Core.Responses;

namespace RDS.Web.Handlers;

public class TransactionHandler(HttpClientService httpClientService) : ITransactionHandler
{
    private readonly Lazy<Task<HttpClient>> _httpClient = new(httpClientService.GetHttpClientAsync);

    private async Task<HttpClient> GetHttpClientAsync()
    {
        return await _httpClient.Value;
    }
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/transactions/create")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
               ?? new Response<Transaction?>(null, 400, "Falha ao criar a transação");
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"v1/transactions/update")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
               ?? new Response<Transaction?>(null, 400, "Não foi possível atualizar a transação");
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"v1/transactions/delete")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
               ?? new Response<Transaction?>(null, 400, "Não foi possível excluir a transação");
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/transactions/byid")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
               ?? new Response<Transaction?>(null, 400, "Não foi possível obter a transação");
    }

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
    {
        const string format = "yyyy-MM-dd";
        var startDate = request.StartDate is not null
            ? request.StartDate.Value.ToString(format)
            : DateTime.Now.GetFirstDay().ToString(format);

        var endDate = request.EndDate is not null
            ? request.EndDate.Value.ToString(format)
            : DateTime.Now.GetLastDay().ToString(format);
        
        var url = $"v1/transactions?startDate={startDate}&endDate={endDate}";

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/transactions/byperiod")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<PagedResponse<List<Transaction>?>>()
               ?? new PagedResponse<List<Transaction>?>(null, 400, "Não foi possível obter as transações");
    }
}