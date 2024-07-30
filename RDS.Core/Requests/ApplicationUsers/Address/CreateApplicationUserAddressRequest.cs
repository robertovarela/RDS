namespace RDS.Core.Requests.ApplicationUsers.Address;

public abstract class CreateApplicationUserAddressRequest : Request
{
    [Required(ErrorMessage = "É obrigatório informar o CEP")]
    [Length(9, 9, ErrorMessage = "O CEP deve conter 9 caracteres")]
    public string PostalCode { get; set; } = null!;

    [Required(ErrorMessage = "É obrigatório informar o logradouro")]
    [MinLength(5, ErrorMessage = "O logradouro deve conter pelo menos 5 caracteres")]
    [MaxLength(160, ErrorMessage = "O logradouro deve conter até 160 caracteres")]
    public string Street { get; set; } = null!;

    [Required(ErrorMessage = "É obrigatório informar o número ou SN se não tiver número")]
    public string Number { get; set; } = string.Empty;
    public string? Complement { get; set; } = string.Empty;

    [Required(ErrorMessage = "É obrigatório informar o bairro")]
    [MinLength(3, ErrorMessage = "O bairro deve conter pelo menos 3 caracteres")]
    [MaxLength(160, ErrorMessage = "O bairro deve conter até 160 caracteres")]
    public string Neighborhood { get; set; } = null!;

    [Required(ErrorMessage = "É obrigatório informar a cidade")]
    [MinLength(5, ErrorMessage = "A cidade deve conter pelo menos 5 caracteres")]
    [MaxLength(160, ErrorMessage = "A cidade deve conter até 160 caracteres")]
    public string City { get; set; } = null!;
    
    [Required(ErrorMessage = "É obrigatório informar o estado")]
    [Length(2, 2, ErrorMessage = "O estado deve conter 2 caracteres")]
    public string State { get; set; } = null!;

    [Required(ErrorMessage = "É obrigatório informar o país")]
    [MinLength(5, ErrorMessage = "O país deve conter pelo menos 5 caracteres")]
    [MaxLength(160, ErrorMessage = "O país deve conter até 160 caracteres")]
    public string Country { get; set; } = null!;

    [Required(ErrorMessage = "Tipo de endereço inválido")]
    public ETypeOfAddress TypeOfAddress { get; set; } = ETypeOfAddress.Main;
}