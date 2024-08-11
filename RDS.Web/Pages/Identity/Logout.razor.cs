namespace RDS.Web.Pages.Identity;

public class LogoutPage : ComponentBase
{
    #region Services
    
    [Inject] private AuthenticationService AuthenticationService { get; set; } = null!;

    #endregion
    
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetDefaultValues();
        await AuthenticationService.LogoutAsync();
        await base.OnInitializedAsync();
        
        await NavigateToLoginAsync();
    }

    #endregion
    
    #region Methods
    
    private async Task NavigateToLoginAsync()
    {
        await Task.Delay(5000);
        NavigationService.NavigateTo("/login");
    }
    
    #endregion
    
}