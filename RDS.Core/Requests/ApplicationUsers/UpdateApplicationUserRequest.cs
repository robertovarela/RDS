namespace RDS.Core.Requests.ApplicationUsers;

public class UpdateApplicationUserRequest : Request
{
    [Required(ErrorMessage = "É obrigatório informar o nome")]
    [MinLength(5, ErrorMessage = "O nome deve conter pelo menos 5 caracteres")]
    [MaxLength(80, ErrorMessage = "O nome deve conter até 80 caracteres")] 
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "É obrigatório informar o E-mail")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; } = null!;

    [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter 11 dígitos.")]
    [Cpf(ErrorMessage = "CPF inválido")]
    public string? Cpf { get; set; } = null!;

    [DataType(DataType.Date, ErrorMessage = "Data inválida")]
    public DateOnly? BirthDate { get; set; }

    // Propriedade auxiliar para o MudDatePicker
    public DateTime? BirthDateAsDateTime
    {
        get => BirthDate?.ToDateTime(TimeOnly.MinValue);
        set => BirthDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : (DateOnly?)null;
    }
}