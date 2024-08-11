namespace RDS.Web.Pages;

public class StartPage : ComponentBase
{
    #region Overrides

    protected override void OnInitialized()
    {
        NavigationService.NavigateTo("/home");
    }

    #endregion
}