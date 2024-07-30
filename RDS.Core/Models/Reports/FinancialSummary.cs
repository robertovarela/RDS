namespace RDS.Core.Models.Reports;

public record FinancialSummary(long UserId, decimal Incomes, decimal Expenses)
{
    public decimal Total => Incomes - (Expenses < 0 ? -Expenses : Expenses);
}