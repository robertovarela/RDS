namespace RDS.Core.Requests.ApplicationUsers;

public class GetAllApplicationUserRoleRequest : Request
{
    public string Token { get; set; } = String.Empty;
    public bool RoleAuthorization { get; set; }
    public List<string> Roles { get; set; } = [];

}