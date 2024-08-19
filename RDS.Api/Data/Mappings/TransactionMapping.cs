namespace RDS.Api.Data.Mappings;

public class TransactionMapping : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transaction");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Title);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);
        builder.Property(x => x.Type)
            .IsRequired()
            .HasColumnType("SMALLINT");
        builder.Property(x => x.Amount)
            .IsRequired()
            .HasColumnType("DECIMAL(18, 2)");
        builder.Property(x => x.CreatedAt)
            .IsRequired();
        builder.Property(x => x.PaidOrReceivedAt)
            .IsRequired(false);
        builder.Property(x => x.CompanyId)
            .IsRequired()
            .HasColumnType("BIGINT");

        builder.HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}