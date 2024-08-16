namespace RDS.Web.Pages;

public class HomePage : ComponentBase
{
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("RDS - Desenvolvimento de SoftWares");
        await StartService.ValidateAccesByToken();
        StartService.SetDefaultValues();
        StateHasChanged();
    }

    #endregion
}