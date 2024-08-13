namespace RDS.Core.Requests.ApplicationUsers;

public class DeleteApplicationUserRoleRequest : Request
{
    public long RoleId { get; set; }
    public string RoleName { get; set; } = null!;
}