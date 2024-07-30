namespace RDS.Core.Requests.Transactions;

public abstract class DeleteTransactionRequest : Request
{
    public long Id { get; set; }
}