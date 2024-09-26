namespace RDS.Web.Pages.Companies;

// ReSharper disable once PartialTypeWithSinglePart
public partial class ListCompaniesPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; set; }
    protected List<Company> Companies { get; private set; } = [];
    private long LoggedUserId { get; set; }
    protected bool IsAdmin { get; set; }
    protected string SearchTerm { get; set; } = string.Empty;
    protected const string AddUrl = "/empresas/adicionar";
    protected const string EditUrl = "/empresas/editar";
    protected const string BackUrl = "/";

    #endregion

    #region Services

    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ICompanyHandler CompanyHandler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Empresas");
        await StartService.ValidateAccesByTokenAsync();
        await StartService.PermissionOnlyAdminOrOwner();
        LoggedUserId = StartService.GetLoggedUserId();
        IsAdmin = await StartService.IsAdminInRolesAsync(LoggedUserId);

        await LoadCompaniesAsync();
    }

    #endregion

    #region Methods

    private async Task LoadCompaniesAsync()
    {
        IsBusy = true;
        try
        {
            var result = IsAdmin switch
            {
                true => await CompanyHandler.GetAllAsync(new GetAllCompaniesRequest()),
                false => await CompanyHandler.GetAllByUserIdAsync(
                    new GetAllCompaniesByUserIdRequest { UserId = LoggedUserId })
            };

            Companies = result.IsSuccess ? result.Data ?? [] : [];
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

    // private async Task LoadCompaniesAsyncByFunc()
    // {
    //     IsBusy = true;
    //     try
    //     {
    //         Func<Task<PagedResponse<List<Company>>>> getCompaniesTask = IsAdmin
    //             ? () => CompanyHandler.GetAllAsync(new GetAllCompaniesRequest())
    //             : () => CompanyHandler.GetAllByUserIdAsync(new GetAllCompaniesByUserIdRequest { UserId = LoggedUserId });
    //
    //         var result = await getCompaniesTask();
    //         if (result.IsSuccess)
    //             Companies = result.Data ?? [];
    //     }
    //     catch (Exception ex)
    //     {
    //         Snackbar.Add(ex.Message, Severity.Error);
    //     }
    //     finally
    //     {
    //         IsBusy = false;
    //     }
    // }

    public async void OnDeleteButtonClickedAsync(long id, string title)
    {
        if (!IsAdmin) return;
        
        var result = await DialogService.ShowMessageBox(
            "ATENÇÃO",
            $"Ao prosseguir a empresa ( {id} - {title} ) será excluída. Esta é uma ação irreversível! Deseja continuar?",
            yesText: "EXCLUIR",
            cancelText: "Cancelar");

        if (result is true)
            await OnDeleteAsync(id);

        StateHasChanged();
    }

    private async Task OnDeleteAsync(long id)
    {
        try
        {
            var request = new DeleteCompanyRequest
            {
                CompanyId = id,
                IsAdmin = IsAdmin
            };
            var result = await CompanyHandler.DeleteAsync(request);
            if (result.IsSuccess)
                Companies.RemoveAll(x => x.Id == id);
            Snackbar.Add(result.Message, result.Data != null ? Severity.Success : Severity.Warning);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    public Func<Company, bool> Filter => company =>
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
            return true;

        if (company.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        if (company.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        if (company.Description.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };

    #endregion
}