namespace RDS.Core.Handlers;

public interface IOrderHandler
{
    Task<Response<Order?>> CreateOrderAsync(CreateOrderRequest request);
    Task<Response<Order?>> ConfirmOrderAsync(ConfirmOrderRequest request);
}