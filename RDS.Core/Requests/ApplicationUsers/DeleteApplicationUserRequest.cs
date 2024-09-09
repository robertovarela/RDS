namespace RDS.Core.Requests.ApplicationUsers
{
    public class DeleteApplicationUserRequest : Request
    {
        public bool RoleAuthorization { get; set; }
    }
}