namespace RDS.Web.Pages.Identity;

// ReSharper disable once PartialTypeWithSinglePart
public partial class LoginPage : ComponentBase
{

    #region Properties

    protected bool IsBusy { get; set; }
    private bool IsShow { get; set; }
    protected LoginRequest LoginModel { get; set; } = new();
    protected InputType PasswordInput { get; set; } = InputType.Password;
    protected string PasswordInputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;

    #endregion
    
    #region Services

    [Inject] public DeviceService DeviceService { get; set; } = null!;
    [Inject] private ILogger<Login> Logger { get; set; } = null!;
    [Inject] public IApplicationUserHandler UserHandler { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        await StartService.VerifyIfLoggedInAsync();
    }

    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        Logger.LogInformation("Starting HandleLogin");
        IsBusy = true;

        try
        {
            var fingerprint = await DeviceService.GetDeviceFingerprint();
            LoginModel.FingerPrint = fingerprint;
            var result = await UserHandler.LoginAsync(LoginModel);
            if (result.IsSuccess)
            {
                Logger.LogInformation("Login successful, navigating to root");
                // StartService.SetLoggedId(result.Data!.UserId);
                // await StartService.SetDefaultValues();
                NavigationService.NavigateToHome();
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

    public void ButtonVisibilityPassword()
    {
        if (IsShow)
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