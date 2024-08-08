namespace RDS.Api.Models;

public class User : ApplicationUser
{
    public new List<IdentityRole<long>>? Roles { get; set; } = [];
}