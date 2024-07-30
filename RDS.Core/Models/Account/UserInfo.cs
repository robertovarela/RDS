namespace RDS.Core.Models.Account;

public class UserInfo
{
    public string Email { get; set; } = null!;
    public bool IsEmailConfirmed { get; set; }
    public Dictionary<string, string> Claims { get; set; } = [];
}