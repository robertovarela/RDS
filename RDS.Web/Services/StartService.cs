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
        SetUrlOrigen("");
    }
    
    public static async Task RefreshToken(string refreshToken, bool showMessage)
    {
        await _manipulateUserStateValuesService.RefreshToken(refreshToken, showMessage);
    }
    
    public static string GetUrlOrigen()
    {
        return _manipulateUserStateValuesService.GetUrlOrigen();
    }
    public static long GetLoggedUserId()
    {
        return _manipulateUserStateValuesService.GetLoggedUserId();
    }
    
    public static long GetSelectedUserId()
    {
        return _manipulateUserStateValuesService.GetSelectedUserId();
    }
    
    public static string GetSelectedUserName()
    {
        return _manipulateUserStateValuesService.GetSelectedUserName();
    }
    
    public static long GetSelectedAddressId()
    {
        return _manipulateUserStateValuesService.GetSelectedAddressId();
    }
    
    public static long GetSelectedCategoryId()
    {
        return _manipulateUserStateValuesService.GetSelectedCategoryId();
    }
    
    public static void SetDefaultValues()
    {
        _manipulateUserStateValuesService.SetDefaultValues();
    }
    
    public static void SetUrlOrigen(string url)
    {
        _manipulateUserStateValuesService.SetUrlOrigen(url);
    }
    public static void SetSelectedUserId(long userId)
    {
        _manipulateUserStateValuesService.SetSelectedUserId(userId);
    }
    
    public static void SetSelectedUserName(string userName)
    {
        _manipulateUserStateValuesService.SetSelectedUserName(userName);
    }
    
    public static void SetSelectedAddressId(long addressId)
    {
        _manipulateUserStateValuesService.SetSelectedAddressId(addressId);
    }
    
    public static void SetSelectedCategoryId(long categoryId)
    {
        _manipulateUserStateValuesService.SetSelectedCategoryId(categoryId);
    }
}