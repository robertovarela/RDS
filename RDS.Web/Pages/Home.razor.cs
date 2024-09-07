namespace RDS.Web.Pages;

public class HomePage : ComponentBase
{
    #region Properties
    private long LoggedUserId { get; set; }
    private long CompanyId { get; set; }
    protected List<CompanyIdNameViewModel> Companies { get; set; } = [];
    protected bool IsAdmin { get; private set; }
    protected bool IsOwner { get; private set; }
    
    #endregion
    
    #region Services

    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    
    #endregion
   
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("RDS - Desenvolvimento de SoftWares");
        await StartService.ValidateAccesByTokenAsync(blockNavigation: false);
        
        LoggedUserId = StartService.GetLoggedUserId();
        IsAdmin = await StartService.IsAdminInRolesAsync(LoggedUserId);
        IsOwner = await StartService.IsOwnerInRolesAsync(LoggedUserId);
        
        StartService.SetIsAdmin(IsAdmin);
        StartService.SetIsOwner(IsOwner);
        
        if (IsOwner || IsAdmin)
        {
            await StartService.SetDefaultValues();
            CompanyId = StartService.GetSelectedCompanyId();
            Companies = StartService.GetUserCompanies();
        }

        //var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        //var user = authState.User;

        // if (user.Identity != null && user.Identity.IsAuthenticated)
        // {
        //     var rolesList = user.Claims
        //         .Where(c => c.Type == ClaimTypes.Role)
        //         .Select(c => c.Value)
        //         .ToList();
        //     roles = string.Join(", ", rolesList);
        //
        //     Console.WriteLine($"Roles do usuário autenticado: {roles}");
        // }
        // else
        // {
        //     roles = "Usuário não autenticado";
        //     Console.WriteLine(roles);
        // }
        //
        // if (user.Identity != null && user.Identity.IsAuthenticated)
        // {
        //     var roles = user.Claims
        //         .Where(c => c.Type.StartsWith("custom_role_"))
        //         .Select(c => c.Value)
        //         .ToList();
        //
        //     Console.WriteLine($"Roles do usuário autenticado: {string.Join(", ", roles)}");
        // }
    }

    #endregion
    
    #region Public Methods
    
    public void SelectCompany(long companyId)
    {
        StartService.SetSelectedCompanyId(companyId);
        Snackbar.Add("Empresa Selecionada", Severity.Success);
    }
    
    #endregion
}