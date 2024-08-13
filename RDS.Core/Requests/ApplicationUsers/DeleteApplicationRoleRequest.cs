namespace RDS.Core.Requests.ApplicationUsers;

public class DeleteApplicationRoleRequest : Request
{
    public string Name { get; set; } = null!;
}