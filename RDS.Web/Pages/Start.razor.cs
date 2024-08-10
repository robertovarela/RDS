namespace RDS.Web.Pages;

public class StartPage : ComponentBase
{
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        await StartService.ValidateAccesByToken();
        StartService.SetDefaultValues();
    }

    #endregion
}