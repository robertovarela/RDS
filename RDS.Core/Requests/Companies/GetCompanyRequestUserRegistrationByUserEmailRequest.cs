namespace RDS.Core.Requests.Companies;

public class GetCompanyRequestUserRegistrationByUserEmailRequest : Request
{
    public long Id { get; set; }
    public string Email { get; set; } = null!;
}