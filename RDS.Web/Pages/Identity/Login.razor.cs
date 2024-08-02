using RDS.Core.Handlers;
using RDS.Core.Requests.Account;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RDS.Core.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using RDS.Web.Services;

namespace RDS.Web.Pages.Identity;

public partial class LoginPage : ComponentBase
{
    #region Services
    
    [Inject]
    ILogger<Login> Logger { get; set; } = null!;

    [Inject]
    AuthenticationService AuthenticationService { get; set; } = null!;
    
    [Inject]
    AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    
    [Inject]
    public UserStateService UserState { get; set; } = null!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Properties

    public bool IsBusy { get; set; } = false;
    public LoginRequest LoginModel { get; set; } = new();

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        
        if (user.Identity is { IsAuthenticated: true })
        {
            NavigationService.NavigateTo("/");
        }
    }

    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        Logger.LogInformation("Starting HandleLogin");
        IsBusy = true;

        try
        {
            var result = await AuthenticationService.LoginAsync(LoginModel);
            if (result)
            {
                Logger.LogInformation("Login successful, navigating to root");
                NavigationService.NavigateTo("/");
            }
            else
            {
                Logger.LogError("Login failed");
                Snackbar.Add("Não foi possível efetuar o login. Verifique o E-mail e a Senha.", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion
    
}