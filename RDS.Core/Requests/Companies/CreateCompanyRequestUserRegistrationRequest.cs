namespace RDS.Core.Requests.Companies;

public class CreateCompanyRequestUserRegistrationRequest : Request
{
    public long Id { get; set; }
    public string CompanyName { get; set; } = null!;
    public long OwnerId { get; set; }
    public string Email { get; set; } = null!;
    public string ConfirmationCode { get; set; } = null!;
    public DateTime ExpirationDate { get; set; }
}