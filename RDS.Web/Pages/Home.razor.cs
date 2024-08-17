namespace RDS.Web.Pages;

public class HomePage : ComponentBase
{
    #region Overrides

    protected override void OnInitialized()
    {
        StartService.SetPageTitle("RDS - Desenvolvimento de SoftWares");
        //await StartService.ValidateAccesByToken();
        StartService.SetDefaultValues();
        StateHasChanged();
    }

    #endregion
}