namespace RDS.Web.Pages.Identity;

public class LogoutPage : ComponentBase
{
    #region Services
    
    [Inject] private AuthenticationService AuthenticationService { get; set; } = null!;

    #endregion
    
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        await AuthenticationService.LogoutAsync();
        await base.OnInitializedAsync();
        await StartService.SetDefaultValues();
        await NavigateToLoginAsync();
    }

    #endregion
    
    #region Methods
    
    private async Task NavigateToLoginAsync()
    {
        await Task.Delay(500);
        NavigationService.NavigateTo("/login");
    }
    
    #endregion
    
}