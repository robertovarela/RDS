namespace RDS.Core.Models.Account;

public class UserRefreshToken(string token = "")
{
    public string Token { get; set; } = token;
}