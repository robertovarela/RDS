namespace RDS.Api.Data.Mappings;

public class IncomesAndExpensesMapping : IEntityTypeConfiguration<IncomesAndExpenses>
{
    public void Configure(EntityTypeBuilder<IncomesAndExpenses> builder)
    {
        builder.HasNoKey();
        builder.ToView("vwGetIncomesAndExpenses");

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(160);

        builder.Property(x => x.Month)
            .IsRequired()
            .HasColumnType("INT");

        builder.Property(x => x.Year)
            .IsRequired()
            .HasColumnType("INT");

        builder.Property(x => x.Incomes)
            .IsRequired()
            .HasColumnType("DECIMAL(18, 2)");

        builder.Property(x => x.Expenses)
            .IsRequired()
            .HasColumnType("DECIMAL(18, 2)");
    }
}