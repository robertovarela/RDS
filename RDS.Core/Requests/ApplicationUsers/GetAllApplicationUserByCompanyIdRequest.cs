namespace RDS.Core.Requests.ApplicationUsers;

public class GetAllApplicationUserByCompanyIdRequest : PagedRequest
{
    public long UserId { get; set; }
    public string? Filter { get; set; } = string.Empty;
}