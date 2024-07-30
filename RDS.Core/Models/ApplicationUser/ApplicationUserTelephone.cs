namespace RDS.Core.Models.ApplicationUser;
public class ApplicationUserTelephone
{
    public long Id { get; set; }
    public int Number { get; set; }
    public ETypeOfPhone Type { get; set; } = ETypeOfPhone.WhatsApp;
    public long UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
}