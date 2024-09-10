namespace RDS.Core.Requests.ApplicationUsers
{
    public class DeleteApplicationUserRequest : Request
    {
        public string Token { get; set; } = String.Empty;
        public bool RoleAuthorization { get; set; }
    }
}