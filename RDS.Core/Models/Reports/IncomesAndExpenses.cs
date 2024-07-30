namespace RDS.Core.Models.Reports;

public record IncomesAndExpenses(long UserId, int Month, int Year, decimal Incomes, decimal Expenses);