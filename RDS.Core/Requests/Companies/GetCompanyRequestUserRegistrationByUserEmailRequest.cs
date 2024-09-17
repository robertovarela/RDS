namespace RDS.Core.Requests.Companies;

public class GetCompanyRequestUserRegistrationByUserEmailRequest : Request
{ 
    public string Email { get; set; } = null!;
}