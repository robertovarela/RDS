namespace RDS.Core.Requests;

public abstract class Request
{
    public long UserId { get; set; }
    public long CompanyId { get; set; }
}