namespace RDS.Core.Models.ApplicationUser;
public class ApplicationUserTelephone
{
    public long Id { get; set; }
    public string Number { get; set; } = String.Empty;
    public ETypeOfPhone Type { get; set; } = ETypeOfPhone.WhatsApp;
    public long UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
}