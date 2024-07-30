namespace RDS.Core.Requests.Orders;

public abstract class ConfirmOrderRequest : Request
{
    public string Number { get; set; } = string.Empty;
}