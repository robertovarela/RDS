namespace RDS.Web.Pages.ApplicationUsers.Configurations.UserRoles;

// ReSharper disable once PartialTypeWithSinglePart
public partial class ListUserRolesPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; private set; }
    protected List<ApplicationUserRole?> RolesFromUser { get; set; } = [];
    protected long UserId { get; set; }
    protected string UserName { get; set; } = StartService.GetSelectedUserName();
    private bool IsAdmin { get; } = StartService.GetIsAdmin();
    private bool IsOwner { get; } = StartService.GetIsOwner();
    protected long CompanyId { get; set; } = StartService.GetSelectedCompanyId();

    protected const string AddUrl = "/usuariosconfiguracao/adicionar-role-usuario";
    private const string CurrentUrl = "/usuariosconfiguracao/lista-roles-do-usuario";
    protected const string BackUrl = "/usuariosconfiguracao/usuarios-para-roles";

    private readonly List<string> _sourceUrl =
    [
        "/usuariosconfiguracao/lista-roles-do-usuario",
        "/usuariosconfiguracao/lista-usuarios-roles",
        "/usuariosconfiguracao/adicionar-role-usuario"
    ];

    #endregion

    #region Services

    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private IApplicationUserConfigurationHandler ApplicationUserConfigurationHandler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Roles");
        //StartService.ValidateSourceUrl(_sourceUrl, CurrentUrl, true, true);
        await StartService.ValidateAccesByTokenAsync();
        if (!await StartService.PermissionOnlyAdminOrOwnerAsync()) return;

        UserId = StartService.GetSelectedUserId();

        await LoadRolesFromUser();
    }

    #endregion

    #region Methods

    private async Task LoadRolesFromUser()
    {
        IsBusy = true;
        RolesFromUser = await StartService.GetRolesFromUserAsync(UserId);
        //RolesFromUser[0].CompanyId = CompanyId;
        IsBusy = false;
    }

    public async void OnDeleteButtonClickedAsync(long userId, string roleName)
    {
        var result = await DialogService.ShowMessageBox(
            "ATENÇÃO",
            $"Ao prosseguir a role ({roleName}) será excluída. Esta é uma ação irreversível! Deseja continuar?",
            yesText: "EXCLUIR",
            cancelText: "Cancelar");

        if (result is true)
            await OnDeleteAsync(userId, roleName);

        StateHasChanged();
    }

    private async Task OnDeleteAsync(long userId, string roleName)
    {
        try
        {
            var request = new DeleteApplicationUserRoleRequest
            {
                CompanyId = userId,
                RoleName = roleName
            };
            var result = await ApplicationUserConfigurationHandler.DeleteUserRoleAsync(request);
            if (result is { IsSuccess: true, StatusCode: 200 })
                RolesFromUser.RemoveAll(x => x != null && x.RoleName == roleName);
            Snackbar.Add(result.Message, result.Data != null ? Severity.Success : Severity.Warning);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    #endregion
}