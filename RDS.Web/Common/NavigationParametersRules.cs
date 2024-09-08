namespace RDS.Web.Common;

public class NavigationParametersRules
{
    public long LoggedUserId { get; } = StartService.GetLoggedUserId();
    private List<CompanyIdNameViewModel> CompanyId { get; set; } = StartService.GetUserCompanies();
    public long UserId { get; } = StartService.GetSelectedUserId();
    public bool IsAdmin { get; } = StartService.GetIsAdmin();
    public bool IsOwner { get; } = StartService.GetIsOwner();
        
    public string EditUrl { get; set; } = string.Empty;
    public string CreateUrl { get; set; } = string.Empty;
    public string SourceUrl { get; set; } = string.Empty;
    public string BackUrl { get; set; } = string.Empty;
    public string CancelUrl { get; set; } = string.Empty;
    
    public string CancelOrBackButtonText { get; set; } = "Cancelar";
}