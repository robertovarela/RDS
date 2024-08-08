namespace RDS.Web.Pages;

public class StartPage : ComponentBase
{
    #region Services
    
    [Inject] private ManipulateUserStateValuesService ManipulateUserStateValues { get; set; } = null!;
    
    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        await StartService.ValidateAccesByToken();
        StartService.SetDefaultValues();
    }

    #endregion
}