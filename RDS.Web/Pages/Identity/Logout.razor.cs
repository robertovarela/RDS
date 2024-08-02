using RDS.Core.Handlers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RDS.Web.Services;

namespace RDS.Web.Pages.Identity;

public partial class LogoutPage : ComponentBase
{
    #region Services
    
    [Inject] AuthenticationService AuthenticationService { get; set; } = null!;
    
    [Inject] NavigationManager NavigationManager { get; set; } = null!;
    
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    #endregion
    
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        await AuthenticationService.LogoutAsync();
        Snackbar.Add("Logout realizado com sucesso!", Severity.Success);
        
        await base.OnInitializedAsync();
    }

    #endregion
}