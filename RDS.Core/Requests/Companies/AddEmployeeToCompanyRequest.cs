namespace RDS.Core.Requests.Companies;

public class AddEmployeeToCompanyRequest : Request
{
    public string Email { get; set; } = null!;
    
    [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter 11 dígitos.")]
    [Cpf(ErrorMessage = "CPF inválido")]
    public string Cpf { get; set; } = null!;
    
    public string? Name { get; set; }
}