using System.Net.Http.Json;
using RDS.Core.Handlers;
using RDS.Core.Models.Reports;
using RDS.Core.Requests.Reports;
using RDS.Core.Responses;

namespace RDS.Web.Handlers;

public class ReportHandler : IReportHandler
{
    private readonly HttpClient _httpClient;

    public ReportHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<Response<List<IncomesAndExpenses>?>> GetIncomesAndExpensesReportAsync(
        GetIncomesAndExpensesRequest request)
    {
        return await _httpClient.GetFromJsonAsync<Response<List<IncomesAndExpenses>?>>($"v1/reports/incomes-expenses")
               ?? new Response<List<IncomesAndExpenses>?>(null, 400, "Não foi possível obter os dados");
    }

    public async Task<Response<List<IncomesByCategory>?>> GetIncomesByCategoryReportAsync(
        GetIncomesByCategoryRequest request)
    {
        return await _httpClient.GetFromJsonAsync<Response<List<IncomesByCategory>?>>($"v1/reports/incomes")
               ?? new Response<List<IncomesByCategory>?>(null, 400, "Não foi possível obter os dados");
    }

    public async Task<Response<List<ExpensesByCategory>?>> GetExpensesByCategoryReportAsync(
        GetExpensesByCategoryRequest request)
    {
        return await _httpClient.GetFromJsonAsync<Response<List<ExpensesByCategory>?>>($"v1/reports/expenses")
               ?? new Response<List<ExpensesByCategory>?>(null, 400, "Não foi possível obter os dados");
    }

    public async Task<Response<FinancialSummary?>> GetFinancialSummaryReportAsync(GetFinancialSummaryRequest request)
    {
        return await _httpClient.GetFromJsonAsync<Response<FinancialSummary?>>($"v1/reports/summary")
               ?? new Response<FinancialSummary?>(null, 400, "Não foi possível obter os dados");
    }
}