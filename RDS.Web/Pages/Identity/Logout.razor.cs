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
        StartService.SetNotLoggedUserId();
        await StartService.SetDefaultValuesAsync();
        await base.OnInitializedAsync();
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