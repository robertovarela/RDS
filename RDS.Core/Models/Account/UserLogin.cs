namespace RDS.Core.Models.Account;

public class UserLogin(string email, string token)
{
    public string Email { get; set; } = email;
    public string Token { get; set; } = token;
}