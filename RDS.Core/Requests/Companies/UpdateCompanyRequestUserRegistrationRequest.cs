namespace RDS.Core.Requests.Companies;

public class UpdateCompanyRequestUserRegistrationRequest : Request
{
    public long Id { get; set; }
    public string ConfirmationCode { get; set; } = null!;
}