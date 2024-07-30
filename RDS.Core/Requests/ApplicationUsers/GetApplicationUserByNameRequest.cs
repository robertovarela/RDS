namespace RDS.Core.Requests.ApplicationUsers;

public class GetApplicationUserByNameRequest :PagedRequestApplicationUser
{
    [Required(ErrorMessage = "É obrigatório informar o nome")]
    [MinLength(5, ErrorMessage = "O nome deve conter pelo menos 5 caracteres")]
    [MaxLength(80, ErrorMessage = "O nome deve conter até 80 caracteres")]
    public string UserName { get; set; } = null!;
}