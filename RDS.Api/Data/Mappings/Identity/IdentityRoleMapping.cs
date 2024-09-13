namespace RDS.Api.Data.Mappings.Identity;

public class IdentityRoleMapping
    : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.ToTable("IdentityRole");
        builder.HasKey(r => r.Id);
        builder.HasIndex(r => r.NormalizedName).IsUnique();
        builder.Property(u => u.Id).IsRequired().HasColumnType("BIGINT").HasColumnOrder(1);
        builder.Property(u => u.Name).HasColumnType("NVARCHAR").HasMaxLength(256).HasColumnOrder(2);
        builder.Property(u => u.NormalizedName).HasColumnType("NVARCHAR").HasMaxLength(256).HasColumnOrder(3);
        builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken().HasColumnType("NVARCHAR(MAX)").HasColumnOrder(4);
    }
}