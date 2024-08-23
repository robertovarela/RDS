using RDS.Core.Requests.Companies;

namespace RDS.Web.Pages.Companies;

public class EditCompanyPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; set; }
    protected UpdateCompanyRequest InputModel { get; set; } = new();
    private long CompanyId { get; set; }

    #endregion

    #region Services

    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public ICompanyHandler CompanyHandler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Editar Empresa");
        await StartService.ValidateAccesByToken();
        CompanyId = StartService.GetSelectedCompanyId();
        await LoadCompany();
    }

    #endregion

    #region Methods

    private async Task LoadCompany()
    {
        try
        {
            IsBusy = true;
            var request = new GetCompanyByIdRequest { CompanyId = CompanyId };
            var response = await CompanyHandler.GetByIdAsync(request);
            if (response is { IsSuccess: true, Data: not null })
                InputModel = new UpdateCompanyRequest
                {
                    CompanyId = response.Data.Id,
                    Name = response.Data.Name,
                    Description = response.Data.Description
                };
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
        InputModel.CompanyId = CompanyId;

        try
        {
            var result = await CompanyHandler.UpdateAsync(InputModel);

            if (result is { IsSuccess: true, Data: not null })
            {
                Snackbar.Add("Empresa atualizada", Severity.Success);
            }
            else
            {
                Snackbar.Add($"A empresa {InputModel.CompanyId}-{InputModel.Name} não foi encontrada", Severity.Warning);
            }

            NavigationService.NavigateTo("/empresas");
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