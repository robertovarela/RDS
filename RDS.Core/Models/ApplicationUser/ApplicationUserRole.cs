namespace RDS.Core.Models.ApplicationUser;

public class ApplicationUserRole : IdentityUserRole<long>
{
    public long UserNId { get; set; }
    public string RoleName { get; set; } = null!;
}