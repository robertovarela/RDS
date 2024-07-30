namespace RDS.Api.Data.Mappings;

public class TelephoneMapping : IEntityTypeConfiguration<ApplicationUserTelephone>
{
    public void Configure(EntityTypeBuilder<ApplicationUserTelephone> builder)
    {
        builder.ToTable("Telephone");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Number)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(20);

        builder.Property(x => x.Type)
            .IsRequired()
            .HasColumnType("SMALLINT");

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnType("BIGINT");
    }
}