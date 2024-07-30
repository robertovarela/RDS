namespace RDS.Core.Requests.ApplicationUsers;

public class CreateApplicationUserRequest : Request
{
    [Required(ErrorMessage = "É obrigatório informar o nome")]
    public string Name { get; set; } = null!;
    
    [Required(ErrorMessage = "É obrigatório informar o E-mail")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "É obrigatório informar a Senha")]
    [PasswordPropertyText]
    public string Password { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "É obrigatório informar a repetição da senha")]
    [PasswordPropertyText]
    public string RepeatPassword { get; set; } = string.Empty;
}