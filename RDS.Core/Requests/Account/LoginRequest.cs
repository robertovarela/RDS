namespace RDS.Core.Requests.Account;

public class LoginRequest : Request
{
    [Required(ErrorMessage = "O E-mail precisa ser informado")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha Inválida")]
    [PasswordPropertyText]
    public string Password { get; set; } = string.Empty;
    
    public IList<ApplicationRole> Roles { get; set; } = [];
}