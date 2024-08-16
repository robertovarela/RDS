namespace RDS.Web.Pages.ApplicationUsers.Configurations.UserRoles;

public partial class ListUserRolesPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    protected List<ApplicationUserRole?> RolesFromUser { get; set; } = [];
    protected long UserId => StartService.GetSelectedUserId();

    #endregion

    #region Services

    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;
    [Inject] public IApplicationUserConfigurationHandler Handler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Roles");
        var urlOrigen = StartService.GetUrlOrigen();
        if (!StartService.GetUrlOrigen().Equals("/usuariosconfiguracao/lista-usuarios-roles")
            && !StartService.GetUrlOrigen().Equals("/usuariosconfiguracao/roles-do-susuario/adicionar-role"))
        {
            Snackbar.Add("Acesso não permitido!", Severity.Error);
            Snackbar.Add("URL de origem não reconhecida", Severity.Error);
            NavigationService.NavigateToLogin();
            return;
        }

        await StartService.ValidateAccesByToken();
        IsBusy = true;
        try
        {
            var request = new GetAllApplicationUserRoleRequest { UserId = UserId };
            var result = await Handler.ListUserRoleAsync(request);
            if (result.IsSuccess)
                RolesFromUser = result.Data ?? [];
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
                UserId = userId,
                RoleName = roleName
            };
            var result = await Handler.DeleteUserRoleAsync(request);
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