namespace RDS.Core.Models.ApplicationUser;

public class ApplicationRole : IdentityRole<long>
{
    public string RoleName { get; set; } = null!;
}