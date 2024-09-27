using RDS.Core.Models.ViewModels.Company;

namespace RDS.Web.Pages;

// ReSharper disable once PartialTypeWithSinglePart
public partial class HomePage : ComponentBase
{
    #region Properties
    private long CompanyId { get; set; }
    protected List<CompanyIdNameViewModel> Companies { get; set; } = [];
    protected bool IsAdmin { get; private set; }
    protected bool IsOwner { get; private set; }
    
    #endregion
    
    #region Services

    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    
    #endregion
   
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("RDS - Desenvolvimento de SoftWares");
        await StartService.ValidateAccesByTokenAsync(blockNavigation: false);
        await StartService.SetDefaultValuesAsync();
        
        LoadStartValues();
    }

    #endregion
    
    #region Public Methods

    private void LoadStartValues()
    {
        IsAdmin = StartService.GetIsAdmin();
        IsOwner = StartService.GetIsOwner();

        if (!IsOwner && !IsAdmin) return;
        
        CompanyId = StartService.GetSelectedCompanyId();
        Companies = StartService.GetUserCompanies();
    }
    
    protected void SelectCompany(long companyId)
    {
        StartService.SetSelectedCompanyId(companyId);
        Snackbar.Add("Empresa Selecionada", Severity.Success);
    }
    
    #endregion
}