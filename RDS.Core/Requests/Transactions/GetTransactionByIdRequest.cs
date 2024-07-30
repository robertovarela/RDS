namespace RDS.Core.Requests.Transactions;

public abstract class GetTransactionByIdRequest : Request
{
    public long Id { get; set; }
}