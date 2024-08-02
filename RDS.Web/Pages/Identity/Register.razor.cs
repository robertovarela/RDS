using Microsoft.AspNetCore.Components;

namespace RDS.Web.Pages.Identity;

public partial class RegisterPage : ComponentBase
{
    #region Dependencies

    [Inject]
    AuthenticationService AuthenticationService { get; set; } = null!;
    
    [Inject]
    AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public IApplicationUserHandler Handler { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    #endregion

    #region Properties

    protected bool IsBusy { get; set; } = false;
    private bool IsShow { get; set; } = false;
    public CreateApplicationUserRequest InputModel { get; set; } = new();

    #endregion

    #region Parameters

    [Parameter]
    public InputType PasswordInput { get; set; } = InputType.Password;
    
    [Parameter]
    public string PasswordInputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;

    

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
        IsBusy = true;
        
        try
        {
            var result = await Handler.CreateAsync(InputModel);

            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message, Severity.Success);
                NavigationManager.NavigateTo("/login");
            }
            else
                Snackbar.Add(result.Message, Severity.Error);
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

    public void ButtonVisibilityPassword()
    {
        if(IsShow)
        {
            IsShow = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else
        {
            IsShow = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }

    #endregion
}