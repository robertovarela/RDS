namespace RDS.Api.Models;

public class User : ApplicationUser
{
    public List<ApplicationRole>? Roles { get; set; } = [];
}