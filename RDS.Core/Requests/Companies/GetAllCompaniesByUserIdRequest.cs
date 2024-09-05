namespace RDS.Core.Requests.Companies;

public class GetAllCompaniesByUserIdRequest : PagedRequest
{
    //public long UserId { get; set; }
    public string Role { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
}