namespace RDS.Core.Requests.Companies;

public class CreateCompanyRequestUserRegistrationRequest : Request
{
    public long Id { get; set; }
    public string CompanyName { get; set; } = null!;
    public long OwnerId { get; set; }
    
    [Required(ErrorMessage = "É obrigatório informar o E-mail")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; } = null!;
    
    [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter 11 dígitos.")]
    [Cpf(ErrorMessage = "CPF inválido")]
    public string Cpf { get; set; } = null!;
    
    [Required(ErrorMessage = "É obrigatório informar o nome")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "É obrigatório informar o prazo de expiração")]
    public int DaysForExpirationDate { get; set; }
}