namespace RDS.Core.Requests.ApplicationUsers
{
    public class DeleteApplicationUserRequest : Request
    {
        public string Role { get; set; } = String.Empty;
    }
}