namespace RDS.Web.Pages.Companies;

// ReSharper disable once PartialTypeWithSinglePart
public partial class AddEmployeeToCompanyPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; private set; }
    protected CreateCompanyRequestUserRegistrationRequest InputModel { get; set; } = new();
    protected ApplicationUser? VerifyUserToEmployee { get; set; }
    private bool UserHasBeenVerified { get; set; }
    protected string SearchFilter { get; set; } = string.Empty;
    protected string OwnerDisplayText { get; set; } = string.Empty;
    protected const string BackUrl = "/empresas";
    protected string Button2Value = "Verificar";

    #endregion

    #region Services

    [Inject] private ICompanyRequestUserRegistrationHandler CompanyHandler { get; set; } = null!;
    [Inject] private IApplicationUserHandler ApplicationUserHandler { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Adicionar Funcionário");
        await StartService.ValidateAccesByTokenAsync();
        await StartService.PermissionOnlyOwnerAsync();
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
        await VerifyUser(SearchFilter);
        StateHasChanged();
    }

    private async Task VerifyUser(string cpf = "")
    {
        IsBusy = true;
        UserHasBeenVerified = false;
        Button2Value = "Verificar";

        try
        {
            var request = new GetApplicationUserByCpfRequest
            {
                Cpf = cpf
            };
            var result = await ApplicationUserHandler.GetByCpfAsync(request);
            if (result is { IsSuccess: true, Data.Id: > 0, StatusCode: 200 })
            {
                VerifyUserToEmployee = result.Data;

                if (VerifyUserToEmployee.Email != InputModel.Email)
                {
                    Snackbar.Add($"E-mail: {VerifyUserToEmployee.Email} é inválido", Severity.Error);
                }
                else
                {
                    UserHasBeenVerified = true;
                    Button2Value = "Adicionar";
                }
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
            StateHasChanged();
        }
    }

    protected async Task OnValidSubmitAsync()
    {
        if (!UserHasBeenVerified)
        {
            await VerifyUser();
        }
        else
        {
            await CreateRequestToUserAsync();
        }
    }

    private async Task CreateRequestToUserAsync()
    {
        IsBusy = true;
        try
        {
            InputModel.CompanyId = StartService.GetSelectedCompanyId();
            //InputModel.CompanyName = 
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