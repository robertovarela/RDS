using System.Net.Http.Json;
using RDS.Core.Handlers;
using RDS.Core.Models;
using RDS.Core.Requests.Orders;
using RDS.Core.Responses;

namespace RDS.Web.Handlers;

public class OrderHandler : IOrderHandler
{
    private readonly HttpClient _httpClient;

    public OrderHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<Response<Order?>> CreateOrderAsync(CreateOrderRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync($"v1/orders", request);
        return await result.Content.ReadFromJsonAsync<Response<Order?>>()
               ?? new Response<Order?>(null, 400, "Não foi possível criar seu pedido");
    }

    public async Task<Response<Order?>> ConfirmOrderAsync(ConfirmOrderRequest request)
    {
        var result = await _httpClient.PutAsJsonAsync($"v1/orders", request);
        return await result.Content.ReadFromJsonAsync<Response<Order?>>()
               ?? new Response<Order?>(null, 400, "Não foi possível atualizar seu pedido");
    }
}