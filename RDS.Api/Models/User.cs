namespace RDS.Api.Models;

public class User : ApplicationUser
{
    public List<IdentityRole<long>>? Roles { get; set; }
}