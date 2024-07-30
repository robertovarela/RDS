using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RDS.Core.Requests.Account;

public class RegisterRequest : Request
{
    [Required(ErrorMessage = "O E-mail precisa ser informado")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha Inválida")]
    [PasswordPropertyText]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "É obrigatório informar a repetição da senha")]
    [PasswordPropertyText]
    public string RepeatPassword { get; set; } = string.Empty;
}