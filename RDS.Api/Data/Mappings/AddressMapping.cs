namespace RDS.Api.Data.Mappings;

public class AddressMapping : IEntityTypeConfiguration<ApplicationUserAddress>
{
    public void Configure(EntityTypeBuilder<ApplicationUserAddress> builder)
    {
        builder.ToTable("Address");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.PostalCode).IsUnique(false);
        builder.HasIndex(x => x.Street).IsUnique(false);

        builder.Property(u => u.Id)
            .IsRequired()
            .HasColumnType("BIGINT")
            .HasColumnOrder(1);

        builder.Property(x => x.PostalCode)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(9);

        builder.Property(x => x.Street)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(160);

        builder.Property(x => x.Number)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(8);

        builder.Property(x => x.Complement)
            .IsRequired(false)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100);

        builder.Property(x => x.Neighborhood)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100);

        builder.Property(x => x.City)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100);

        builder.Property(x => x.State)
            .IsRequired()
            .HasColumnType("CHAR")
            .HasMaxLength(2);

        builder.Property(x => x.Country)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100);

        builder.Property(x => x.TypeOfAddress)
            .IsRequired()
            .HasColumnType("SMALLINT")
            .HasMaxLength(1);

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnType("BIGINT");
    }
}