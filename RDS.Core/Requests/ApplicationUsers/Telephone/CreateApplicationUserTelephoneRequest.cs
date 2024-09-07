namespace RDS.Core.Requests.ApplicationUsers.Telephone
{
    public class CreateApplicationUserTelephoneRequest : Request
    {
        [Required(ErrorMessage = "É obrigatório informar o telefone")]
        [Length(15, 15, ErrorMessage = "O telefone deve conter 11 caracteres")]
        public string Number { get; set; } = null!;

        [Required(ErrorMessage = "Tipo de telefone inválido")]
        public ETypeOfPhone Type { get; set; }
    }
}