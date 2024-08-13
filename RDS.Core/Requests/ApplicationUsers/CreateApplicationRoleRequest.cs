namespace RDS.Core.Requests.ApplicationUsers;

public class CreateApplicationRoleRequest : Request
{
    [Required(ErrorMessage = "É obrigatório informar o nome")]
    public string Name { get; set; } = null!;
}