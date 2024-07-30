namespace RDS.Core.Models.Reports;

public record IncomesByCategory(long UserId, string Category, int Year, decimal Incomes);