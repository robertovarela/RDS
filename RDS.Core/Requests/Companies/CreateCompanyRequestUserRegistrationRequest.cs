namespace RDS.Core.Requests.Companies;

public class CreateCompanyRequestUserRegistrationRequest : Request
{
    public long Id { get; set; }
    public string CompanyName { get; set; } = null!;
    public long OwnerId { get; set; }
    public string Email { get; set; } = null!;
    public int DaysForExpirationDate { get; set; }
}