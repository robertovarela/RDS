namespace RDS.Api.Data.Mappings;

public class IncomesByCategoryMapping : IEntityTypeConfiguration<IncomesByCategory>
{
    public void Configure(EntityTypeBuilder<IncomesByCategory> builder)
    {
        builder.HasNoKey();
        builder.ToView("vwGetIncomesByCategory");

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(160);

        builder.Property(x => x.Category)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);

        builder.Property(x => x.Year)
            .IsRequired()
            .HasColumnType("INT");

        builder.Property(x => x.Incomes)
            .IsRequired()
            .HasColumnType("DECIMAL(18, 2)");
    }
}