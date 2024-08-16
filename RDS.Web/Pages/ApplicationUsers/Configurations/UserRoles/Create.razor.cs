namespace RDS.Web.Pages.ApplicationUsers.Configurations.UserRoles;

public partial class CreateUserRolePage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; }
    public CreateApplicationUserRoleRequest InputModel { get; set; } = new();
    public List<ApplicationUserRole?> Roles { get; set; } = [];
    protected long UserId => StartService.GetSelectedUserId();
    private const string UrlOrigen = "/usuariosconfiguracao/roles-do-susuario/adicionar-role";

    #endregion

    #region Services

    [Inject] public IApplicationUserConfigurationHandler UserRoleHandler { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        await StartService.ValidateAccesByToken();
        StartService.SetUrlOrigen(UrlOrigen);
        IsBusy = true;

        try
        {
            InputModel.UserId = UserId;
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

    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await UserRoleHandler.CreateUserRoleAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message, Severity.Success);
                NavigationService.NavigateTo("/usuariosconfiguracao/lista-roles-do-susuario");
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