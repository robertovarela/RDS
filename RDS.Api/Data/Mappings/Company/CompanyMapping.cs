namespace RDS.Api.Data.Mappings.Company;

public class CompanyMapping : IEntityTypeConfiguration<Core.Models.Company.Company>
{
    public void Configure(EntityTypeBuilder<Core.Models.Company.Company> builder)
    {
        builder.ToTable("Company");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);
        
        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(255);
        
        builder.Property(x => x.OwnerId)
            .IsRequired()
            .HasColumnType("BIGINT");
        
        builder.HasOne(c => c.Owner)
            .WithMany() // Sem navegação inversa
            .HasForeignKey(c => c.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(c => c.CompanyUsers)
            .WithOne(cu => cu.Company)
            .HasForeignKey(cu => cu.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}