namespace RDS.Core.Models.Company;

public class CompanyUser
{
    public long CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    public long UserId { get; set; }
    public ApplicationUser.ApplicationUser User { get; set; } = null!;
        
    public bool IsAdmin { get; set; } // Opcional: para indicar se o usuário é um administrador na empresa
}