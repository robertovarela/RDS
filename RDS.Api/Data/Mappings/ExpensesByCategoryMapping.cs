namespace RDS.Api.Data.Mappings;

public class ExpensesByCategoryMapping : IEntityTypeConfiguration<ExpensesByCategory>
{
    public void Configure(EntityTypeBuilder<ExpensesByCategory> builder)
    {
        builder.HasNoKey();
        builder.ToView("vwGetExpensesByCategory");

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

        builder.Property(x => x.Expenses)
            .IsRequired()
            .HasColumnType("DECIMAL(18, 2)");
    }
}