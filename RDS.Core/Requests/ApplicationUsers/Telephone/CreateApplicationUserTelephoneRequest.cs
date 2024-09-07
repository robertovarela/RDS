namespace RDS.Core.Requests.ApplicationUsers.Telephone
{
    public class CreateApplicationUserTelephoneRequest : Request
    {
        [Required(ErrorMessage = "É obrigatório informar o telefone")]
        [Length(11, 11, ErrorMessage = "O telefone deve conter 11 caracteres")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Tipo de telefone inválido")]
        public ETypeOfPhone Type { get; set; }
    }
}