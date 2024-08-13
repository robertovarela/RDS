namespace RDS.Core.Requests.ApplicationUsers;

public class CreateApplicationUserRoleRequest : Request
{
    public long RoleId { get; set; }
    public string RoleName { get; set; } = null!;

}