namespace RDS.Core.Requests.Companies;

public class GetAllCompaniesByUserIdRequest : PagedRequest
{
    public long UserId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
}