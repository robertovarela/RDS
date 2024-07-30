namespace RDS.Core.Requests.ApplicationUsers;

public class GetApplicationUserByCpfRequest
{
    [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter 11 dígitos.")]
    [Cpf(ErrorMessage = "CPF inválido")]
    public string Cpf { get; set; } = null!;
}