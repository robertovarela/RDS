namespace RDS.Core.Requests.Companies;

public class UpdateCompanyRequestUserRegistrationRequest : Request
{
    public string Email { get; set; } = null!;
    public string ConfirmationCode { get; set; } = null!;
}