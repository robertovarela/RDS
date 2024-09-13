namespace RDS.Core.Models.ApplicationUser;

public class ApplicationRole : IdentityRole<long>
{
    public ApplicationRole()
    {
    }

    public ApplicationRole(string roleName)
    {
        RoleName = roleName;
    }
    public long CompanyId  { get; set; }

    public string RoleName { get; set; } = null!;
}