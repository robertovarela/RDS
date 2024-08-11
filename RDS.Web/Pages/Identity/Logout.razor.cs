namespace RDS.Web.Pages.Identity;

public partial class LogoutPage : ComponentBase
{
    #region Services
    
    [Inject] private AuthenticationService AuthenticationService { get; set; } = null!;
    
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    #endregion
    
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetDefaultValues();
        await AuthenticationService.LogoutAsync();
        Snackbar.Add("Logout realizado com sucesso!", Severity.Success);
        
        await base.OnInitializedAsync();
    }

    #endregion
}