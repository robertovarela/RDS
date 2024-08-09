namespace RDS.Core.Requests.Account;

public class RefreshTokenRequest : Request
{
    public string Token { get; set; } = string.Empty;
}