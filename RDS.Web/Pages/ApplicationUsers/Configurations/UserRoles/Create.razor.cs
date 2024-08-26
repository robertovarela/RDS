namespace RDS.Web.Pages.ApplicationUsers.Configurations.UserRoles;

public partial class CreateUserRolePage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; set; }
    protected CreateApplicationUserRoleRequest InputModel { get; set; } = new();
    protected List<ApplicationUserRole?> Roles { get; set; } = [];
    private long UserId { get; set; }
    private string UserName { get; set; } = StartService.GetSelectedUserName();
    private const string ListUrl = "/usuariosconfiguracao/roles-do-usuario/lista-roles-do-usuario";
    private readonly List<string> _urlOrigen = ["/usuariosconfiguracao/roles-do-usuario/adicionar-role"];

    #endregion

    #region Services

    [Inject] private IApplicationUserConfigurationHandler UserRoleHandler { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Nova Role");
        await StartService.ValidateAccesByTokenAsync();
        if(!await StartService.PermissionOnlyAdmin()) return;
        UserId = StartService.GetSelectedUserId();
        StartService.SetSourceUrl(_urlOrigen);
        IsBusy = true;
        await LoadRolesFromUser();
        StartService.SetSelectedUserId(0);
        StartService.SetSelectedUserName("");
    }

    #endregion

    #region Methods

    private async Task LoadRolesFromUser()
    {
        try
        {
            InputModel.CompanyId = UserId;
            var request = new GetAllApplicationUserRoleRequest
            {
                UserId = UserId
            };

            var result = await UserRoleHandler.ListRoleToAddUserAsync(request);
            if (result.IsSuccess)
            {
                Roles = result.Data ?? [];
                InputModel.RoleName = Roles.FirstOrDefault()?.RoleName ?? "";
                StateHasChanged();
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
    
    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await UserRoleHandler.CreateUserRoleAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message, Severity.Success);
                StartService.LinkToUrlUserRole(UserId, UserName, ListUrl);
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