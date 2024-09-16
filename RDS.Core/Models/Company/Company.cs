namespace RDS.Core.Models.Company;

public class Company
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public long OwnerId { get; set; }
    public virtual ApplicationUser.ApplicationUser Owner { get; set; } = null!; // Propriedade de navegação

    public virtual ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();
}