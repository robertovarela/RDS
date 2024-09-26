namespace RDS.Web.Pages.Companies;

public class CreateCompanyPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; private set; }
    protected bool SelectedCompanyOwner { get; set; }
    protected CreateCompanyRequest InputModel { get; set; } = new();
    protected List<AllUsersViewModel> FilteredUsers { get; set; } = [];
    protected string SearchFilter { get; set; } = string.Empty;
    protected string OwnerDisplayText { get; set; } = string.Empty;
    protected const string BackUrl = "/empresas";

    #endregion

    #region Services

    [Inject] private ICompanyHandler CompanyHandler { get; set; } = null!;
    [Inject] private IApplicationUserHandler ApplicationUserHandler { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Nova Empresa");
        await StartService.ValidateAccesByTokenAsync();
        await StartService.PermissionOnlyAdmin();
    }

    #endregion

    #region Methods

    protected void HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            OnSearch();
        }
    }
    protected async void OnSearch()
    {
        await LoadUsers(SearchFilter);
        StateHasChanged();
    }

    private async Task LoadUsers(string filter = "")
    {
        IsBusy = true;

        try
        {
            var request = new GetAllApplicationUserRequest
            {
                Filter = filter
            };
            var result = await ApplicationUserHandler.GetAllAsync(request);
            if (result.IsSuccess)
            {
                FilteredUsers = result.Data ?? [];
                InputModel.OwnerId = FilteredUsers.FirstOrDefault()?.Id ?? 0;
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
    
    protected async Task OnSelect(long userId)
    {
        SelectedCompanyOwner = true;
        await LoadUsers(filter: userId.ToString());
        OwnerDisplayText = FilteredUsers.FirstOrDefault()?.Id + " - " + FilteredUsers.FirstOrDefault()?.Name;
        StateHasChanged();
    }
    
    protected async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        try
        {
            var result = await CompanyHandler.CreateAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message, Severity.Success);
                NavigationService.NavigateTo(BackUrl);
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