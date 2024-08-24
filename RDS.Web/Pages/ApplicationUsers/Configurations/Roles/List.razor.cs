namespace RDS.Web.Pages.ApplicationUsers.Configurations.Roles;

// ReSharper disable once PartialTypeWithSinglePart
public partial class ListRolesPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; }
    protected List<ApplicationRole?> Roles { get; set; } = [];
    protected string SearchTerm { get; set; } = string.Empty;
    protected const string AddUrl = "/usuariosconfiguracao/adicionar-role";

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
        await StartService.ValidateAccesByTokenAsync();
        IsBusy = true;
        try
        {
            var result = await Handler.ListRoleAsync();
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

    public async void OnDeleteButtonClickedAsync(string roleName)
    {
        var result = await DialogService.ShowMessageBox(
            "ATENÇÃO",
            $"Ao prosseguir a role ( {roleName} ) será excluída. Esta é uma ação irreversível! Deseja continuar?",
            yesText: "EXCLUIR",
            cancelText: "Cancelar");

        if (result is true)
            await OnDeleteAsync(roleName);

        StateHasChanged();
    }

    private async Task OnDeleteAsync(string roleName)
    {
        try
        {
            var request = new DeleteApplicationRoleRequest { Name = roleName };
            var result = await Handler.DeleteRoleAsync(request);
            if (result is { IsSuccess: true, StatusCode: 200 })
                Roles.RemoveAll(x => x != null && x.Name == roleName);
            Snackbar.Add(result.Message, result.Data != null ? Severity.Success : Severity.Warning);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    public Func<ApplicationRole?, bool> Filter => role =>
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
            return true;

        return role!.Name != null && role.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);
    };

    #endregion
}