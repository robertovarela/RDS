namespace RDS.Core.Models.Reports;

public record ExpensesByCategory(long UserId, string Category, int Year, decimal Expenses);