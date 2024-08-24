namespace RDS.Core.Requests.Companies;

public class GetAllComaniesByUserIdRequest : PagedRequest
{
    public long UserId { get; set; }
}