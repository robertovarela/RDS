namespace RDS.Core.Models.ApplicationUser;

public class ApplicationUserAddress
{
    public long Id { get; set; }
    public string PostalCode { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string Number { get; set; } = string.Empty;
    public string? Complement { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = null!;
    public string City { get; set; } = null!;
    
    public string State { get; set; } = null!;
    public string Country { get; set; } = null!;
    public ETypeOfAddress TypeOfAddress { get; set; } = ETypeOfAddress.Main;
    public long UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
}