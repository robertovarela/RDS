using Microsoft.AspNetCore.Components;
using MudBlazor;
using RDS.Core.Handlers;
using RDS.Core.Models.Reports;
using RDS.Core.Requests.Reports;

namespace RDS.Web.Pages.Dashboard;
public partial class DashboardPage : ComponentBase
{
    #region Properties

    public bool ShowValues { get; set; } = false;
    public FinancialSummary? Summary { get; set; }

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public IReportHandler Handler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        var request = new GetFinancialSummaryRequest();
        var result = await Handler.GetFinancialSummaryReportAsync(request);
        if (result.IsSuccess)
            Summary = result.Data;
    }

    #endregion

    #region Methods

    public void ToggleShowValues()
        => ShowValues = !ShowValues;

    #endregion
}