namespace RDS.Api.Data.Mappings;

public class OrderMapping : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Number)
            .IsRequired(false)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);

        builder.Property(x => x.ExternalReference)
            .IsRequired(false)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("DATETIME");

        builder.Property(x => x.UpdatedAt)
            .IsRequired()
            .HasColumnType("DATETIME");

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnType("TINYINT");

        builder.Property(x => x.Gateway)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnType("TINYINT");

        builder.Property(x => x.Amount)
            .IsRequired()
            .HasColumnType("DECIMAL(18, 2)");

        builder.Property(x => x.CompanyId)
            .IsRequired()
            .HasColumnType("BIGINT");
    }
}