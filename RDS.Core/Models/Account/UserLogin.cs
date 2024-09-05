namespace RDS.Core.Models.Account;

public class UserLogin(long userId, string email, string token)
{
    public long UserId { get; set; } = userId;
    public string Email { get; set; } = email;
    public string Token { get; set; } = token;
}