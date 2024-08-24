namespace RDS.Web.Pages.ApplicationUsers.Configurations.Roles;

public partial class CreateRolePage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; }
    public CreateApplicationRoleRequest InputModel { get; set; } = new();

    #endregion

    #region Services

    [Inject] public IApplicationUserConfigurationHandler RoleHandler { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides
    
    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Nova Role");
        await StartService.ValidateAccesByTokenAsync();
    }
    
    #endregion
    
    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await RoleHandler.CreateRoleAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message, Severity.Success);
                NavigationService.NavigateTo("usuariosconfiguracao/roles");
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

    #endregion
}