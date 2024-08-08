namespace RDS.Core.Requests.ApplicationUsers;

    public class GetAllApplicationUserRequest : PagedRequest
    {
        public string? Filter { get; set; } = string.Empty;
    }