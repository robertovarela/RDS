#pragma warning disable CS8602 // Dereference of a possibly null reference.
namespace RDS.Web.Services;

public abstract class StartService
{
    private static ManipulateUserStateValuesService? _manipulateUserStateValuesService;
    public static void Initialize(ManipulateUserStateValuesService manipulateUserStateValuesService)
    {
        _manipulateUserStateValuesService = manipulateUserStateValuesService;
    }
    
    public static async Task ValidateAccesByToken()
    {
        await _manipulateUserStateValuesService.ValidateAccessByToken();
        SetSourceUrl([]);
    }

    public static bool ValidateSourceUrl(List<string> sourceUrl, 
        string currentUrl, 
        bool navigateToAccessNotAllowed = true, 
        bool showMessage = true) =>
        _manipulateUserStateValuesService.ValidateSourceUrl(sourceUrl, currentUrl, navigateToAccessNotAllowed, showMessage);
    public static async Task RefreshToken(string refreshToken, bool showMessage) =>
        await _manipulateUserStateValuesService.RefreshToken(refreshToken, showMessage);
    public static string GetPageTitle() => _manipulateUserStateValuesService.GetPageTitle();
    public static List<string> GetSourceUrl() => _manipulateUserStateValuesService.GetSourceUrl();
    public static string GetCurrentUrl() => _manipulateUserStateValuesService.GetCurrentUrl();
    public static long GetLoggedUserId() => _manipulateUserStateValuesService.GetLoggedUserId();
    public static long GetSelectedUserId() => _manipulateUserStateValuesService.GetSelectedUserId();
    public static string GetSelectedUserName() => _manipulateUserStateValuesService.GetSelectedUserName();
    public static long GetSelectedAddressId() => _manipulateUserStateValuesService.GetSelectedAddressId();
    public static long GetSelectedCompanyId() => _manipulateUserStateValuesService.GetSelectedCompanyID();
    public static long GetSelectedCategoryId() => _manipulateUserStateValuesService.GetSelectedCategoryId();
    public static long GetSelectedTransactionId() => _manipulateUserStateValuesService.GetSelectedTransactionId();
    
    public static void SetDefaultValues() => _manipulateUserStateValuesService.SetDefaultValues();
    public static void SetPageTitle(string title) => _manipulateUserStateValuesService.SetPageTitle(title);
    public static void SetSourceUrl(List<string> urlList) => _manipulateUserStateValuesService.SetSourceUrl(urlList);
    public static void SetCurrentUrl(string url) => _manipulateUserStateValuesService.SetCurrentUrl(url);
    public static void SetSelectedUserId(long userId) => _manipulateUserStateValuesService.SetSelectedUserId(userId);
    public static void SetSelectedUserName(string userName) => _manipulateUserStateValuesService.SetSelectedUserName(userName);
    public static void SetSelectedAddressId(long addressId) => _manipulateUserStateValuesService.SetSelectedAddressId(addressId);
    public static void SetSelectedCompanyId(long companyId) => _manipulateUserStateValuesService.SetSelectedCompanyId(companyId);
    public static void SetSelectedCategoryId(long categoryId) => _manipulateUserStateValuesService.SetSelectedCategoryId(categoryId);
    public static void SetSelectedTransactionId(long transactionId) => _manipulateUserStateValuesService.SetSelectedTransactionId(transactionId);
    
    // This methods are used to set and get the selected user id and name in the application and are used in the LinkUserStateService
    public static void LinkToUrlUser(string url = "", long userId = 0)
    {
        SetSelectedUserId(userId);
        NavigationService.NavigateTo(url);
    }
        
    public static void LinkToUrlUserRole(long userId, string userName, string url = "")
    {
        SetSelectedUserId(userId);
        SetSelectedUserName(userName);
        NavigationService.NavigateTo(url);
    }
   
    public static void LinkToUrlAddress(string url = "", string urlOrigen = "", long userId = 0, long addressId = 0)
    {
        SetSelectedUserId(userId);
        SetSelectedAddressId(addressId);
        NavigationService.NavigateTo(url);
    }
        
    public static void LinkToUrlCategory(string url = "", long companyId = 0, long categoryId = 0)
    {
        SetSelectedUserId(companyId);
        SetSelectedCategoryId(categoryId);
        NavigationService.NavigateTo(url);
    }

    public static void LinkToUrlTransaction(string url = "", long companyId = 0, long transactionId = 0)
    {
        SetSelectedUserId(companyId);
        SetSelectedTransactionId(transactionId);
        NavigationService.NavigateTo(url);
    }
    // End user state methods
}