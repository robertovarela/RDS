namespace RDS.Core.Models.ApplicationUser;

public class ApplicationUserRole : IdentityUserRole<long>
{
    public string RoleName { get; set; } = null!;
}