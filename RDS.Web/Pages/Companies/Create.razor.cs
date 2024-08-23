using RDS.Core.Requests.Companies;

namespace RDS.Web.Pages.Companies;

public class CreateCompanyPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; private set; }
    protected bool OwnerSelected { get; set; }
    protected CreateCompanyRequest InputModel { get; set; } = new();
    protected List<Company> Companies { get; set; } = [];
    protected List<ApplicationUser> FilteredUsers { get; set; } = [];
    protected string SearchFilter { get; set; } = string.Empty;
    public string OwnerDisplayText { get; set; } = string.Empty;

    #endregion

    #region Services

    [Inject] public ICompanyHandler CompanyHandler { get; set; } = null!;
    [Inject] public IApplicationUserHandler ApplicationUserHandler { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Nova Empresa");
        await StartService.ValidateAccesByToken();
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

    protected async Task OnSelect(long userId)
    {
        OwnerSelected = true;
        await LoadUsers(filter: userId.ToString());
        OwnerDisplayText = FilteredUsers.FirstOrDefault()?.Id + " - " + FilteredUsers.FirstOrDefault()?.Name;
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
    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        try
        {
            var result = await CompanyHandler.CreateAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message, Severity.Success);
                NavigationService.NavigateTo("/empresas");
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