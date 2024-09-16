namespace RDS.Api.Data.Mappings.Company;

public class
    CompanyRequestUserRegistrationMapping : IEntityTypeConfiguration<Core.Models.Company.CompanyRequestUserRegistration>
{
    public void Configure(EntityTypeBuilder<Core.Models.Company.CompanyRequestUserRegistration> builder)
    {
        builder.ToTable("CompanyUserRegistration");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.CompanyId, x.Email })
            .IsUnique();

        builder.Property(x => x.CompanyId)
            .IsRequired()
            .HasColumnType("BIGINT");

        builder.Property(x => x.CompanyName)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);

        builder.Property(x => x.OwnerId)
            .IsRequired()
            .HasColumnType("BIGINT");

        builder.Property(x => x.Email)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(255);

        builder.Property(x => x.ConfirmationCode)
            .IsRequired()
            .HasColumnType("nvarchar")
            .HasMaxLength(29);

        builder.Property(x => x.ExpirationDate)
            .IsRequired()
            .HasColumnType("DATETIME");

        builder.Property(x => x.ConfirmationDate)
            .IsRequired(false)
            .HasColumnType("DATETIME");
    }
}