namespace RDS.Core.Requests.Companies;

public class GetAllCompanyIdByUserIdRequest : PagedRequest
{
    public long UserId { get; set; }
}