using RDS.Core.Requests.ApplicationUsers.Telephone;

namespace RDS.Web.Pages.ApplicationUsers.Configurations.UserRoles;

public partial class ListUserRolesPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    protected List<ApplicationUserRole?> Roles { get; set; } = [];
    protected string SearchTerm { get; set; } = string.Empty;

    #endregion

    #region Services

    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;
    [Inject] public IApplicationUserConfigurationHandler Handler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        await StartService.ValidateAccesByToken();
        IsBusy = true;
        try
        {
            var request = new GetAllApplicationUserRoleRequest();
            var result = await Handler.ListUserRoleAsync(request);
            if (result.IsSuccess)
                Roles = result.Data ?? [];
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

    public async void OnDeleteButtonClickedAsync(long userId, long roleId, string roleName)
    {
        var result = await DialogService.ShowMessageBox(
            "ATENÇÃO",
            $"Ao prosseguir a role ({roleName}) será excluída. Esta é uma ação irreversível! Deseja continuar?",
            yesText: "EXCLUIR",
            cancelText: "Cancelar");

        if (result is true)
            await OnDeleteAsync(userId, roleId, roleName);

        StateHasChanged();
    }

    public async Task OnDeleteAsync(long userId, long roleId, string roleName)
    {
        try
        {
            var request = new DeleteApplicationUserRoleRequest
            {
                UserId = userId,
                RoleId = roleId,
                RoleName = roleName
            };
            var result = await Handler.DeleteUserRoleAsync(request);
            if (result.IsSuccess)
                Roles.RemoveAll(x => x != null && x.RoleId == roleId);
            Snackbar.Add(result.Message, result.Data != null ? Severity.Success : Severity.Warning);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    public Func<ApplicationRole, bool> Filter => role =>
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
            return true;

        if (role.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        return role.Name != null && role.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);
    };

    #endregion
}