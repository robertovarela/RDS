namespace RDS.Web.Pages.ApplicationUsers.Configurations.UserRoles;

// ReSharper disable once PartialTypeWithSinglePart
public partial class CreateUserRolePage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; set; }
    protected CreateApplicationUserRoleRequest InputModel { get; set; } = new();
    protected List<ApplicationUserRole?> UserRoles { get; private set; } = [];
    protected List<ApplicationRole?> Roles { get; private set; } = [];
    private long LoggedUserId { get; set; } = StartService.GetLoggedUserId();
    private long UserId { get; set; } = StartService.GetSelectedUserId();
    private long CompanyId { get; set; } = StartService.GetSelectedCompanyId();
    private string UserName { get; set; } = StartService.GetSelectedUserName();
    private List<ApplicationUserRole?> RolesFromLoggedUserId { get; set; } = [];
    private string Token { get; set; } = string.Empty; 
    private const string ListUrl = "/usuariosconfiguracao/lista-roles-do-usuario";
    private readonly List<string> _urlOrigen = ["/usuariosconfiguracao/adicionar-role-usuario"];

    #endregion

    #region Services

    [Inject] private IApplicationUserConfigurationHandler UserRoleHandler { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Nova Role");
        await StartService.ValidateAccesByTokenAsync();
        if(!await StartService.PermissionOnlyAdminOrOwnerAsync()) return;
        
        StartService.SetSourceUrl(_urlOrigen);
        
        Token = await StartService.GetTokenFromLocalStorageAsync();
        await GetRolesFromLoggedUserIdAsync();
        await LoadRolesFromUser(Token);
    }

    #endregion

    #region Methods

    private async Task GetRolesFromLoggedUserIdAsync()
    {
        RolesFromLoggedUserId = await StartService.GetRolesFromUserAsync(LoggedUserId);
    }
    private async Task LoadRolesFromUser(string token)
    {
        try
        {
            IsBusy = true;
            var request = new GetAllApplicationUserRoleRequest
            {
                UserId = UserId,
                CompanyId = CompanyId,
                Roles = ["Admin", "Owner"],
                Token = token,
                RoleAuthorization = true
            };

            var result = await UserRoleHandler.ListRolesForAddToUserAsync(request);
            if (result.IsSuccess)
            {
                UserRoles = result.Data ?? [];
                if (UserRoles.Count != 0)
                {
                    InputModel.UserId = UserId;
                    InputModel.CompanyId = CompanyId;
                    InputModel.RoleId = UserRoles.FirstOrDefault()!.RoleId!;
                    InputModel.RoleName = UserRoles.FirstOrDefault()!.RoleName; 
                }
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
            var result = await UserRoleHandler.CreateUserRoleAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message, Severity.Success);
                StartService.LinkToUrlUserRole(UserId, UserName, CompanyId,ListUrl);
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