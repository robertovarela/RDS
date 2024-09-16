namespace RDS.Core.Models.Company;

public class CompanyRequestUserRegistration
{
    public long Id { get; set; }
    public long CompanyId { get; set; }
    public string CompanyName { get; set; } = null!;
    public long OwnerId { get; set; }
    public string Email { get; set; } = null!;
    public string ConfirmationCode { get; set; } = null!;
    public DateTime ExpirationDate { get; set; }
    public DateTime? ConfirmationDate { get; set; }
}