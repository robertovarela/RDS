using System.Net.Http.Json;
using RDS.Core.Common.Extensions;
using RDS.Core.Handlers;
using RDS.Core.Models;
using RDS.Core.Requests.Transactions;
using RDS.Core.Responses;

namespace RDS.Web.Handlers;

public class TransactionHandler : ITransactionHandler
{
    private readonly HttpClient _httpClient;

    public TransactionHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync("v1/transactions", request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
               ?? new Response<Transaction?>(null, 400, "Não foi possível criar sua transação");
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        var result = await _httpClient.PutAsJsonAsync($"v1/transactions/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
               ?? new Response<Transaction?>(null, 400, "Não foi possível atualizar sua transação");
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        var result = await _httpClient.DeleteAsync($"v1/transactions/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
               ?? new Response<Transaction?>(null, 400, "Não foi possível excluir sua transação");
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
        => await _httpClient.GetFromJsonAsync<Response<Transaction?>>($"v1/transactions/{request.Id}")
           ?? new Response<Transaction?>(null, 400, "Não foi possível obter a transação");

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

        return await _httpClient.GetFromJsonAsync<PagedResponse<List<Transaction>?>>(url)
            ?? new PagedResponse<List<Transaction>?>(null, 400, "Não foi possível obter as transações");
    }
}