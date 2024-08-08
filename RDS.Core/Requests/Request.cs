namespace RDS.Core.Requests;

public abstract class Request
{
    public long UserId { get; set; }
    public string Token { get; set; } = string.Empty;
}