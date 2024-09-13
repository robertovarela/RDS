namespace RDS.Api.Data.Mappings.Identity;

public class IdentityUserRoleMapping
    : IEntityTypeConfiguration<ApplicationUserRole>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        builder.ToTable("IdentityUserRole");
        builder.HasKey(r => new { r.UserId, r.RoleId, r.CompanyId });
    }
}