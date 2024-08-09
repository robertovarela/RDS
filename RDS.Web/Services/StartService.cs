#pragma warning disable CS8602 // Dereference of a possibly null reference.
namespace RDS.Web.Services;

public abstract class StartService
{
    private static ManipulateUserStateValuesService? _manipulateUserStateValuesService;
    public static void Initialize(ManipulateUserStateValuesService manipulateUserStateValuesService)
    {
        _manipulateUserStateValuesService = manipulateUserStateValuesService;
    }
    
    public static async Task<string> ValidateAccesByToken()
    {
        return await _manipulateUserStateValuesService.ValidateAccessByToken();
    }
    
    public static async Task RefreshToken(string refreshToken, bool showMessage = true)
    {
        await _manipulateUserStateValuesService.RefreshToken(refreshToken, showMessage);
    }
    public static long GetLoggedUserId()
    {
        return _manipulateUserStateValuesService.GetLoggedUserId();
    }
    
    public static long GetSelectedUserId()
    {
        return _manipulateUserStateValuesService.GetSelectedUserId();
    }
    
    public static long GetSelectedAddressId()
    {
        return _manipulateUserStateValuesService.GetSelectedAddressId();
    }
    
    public static long GetSelectedCategoriId()
    {
        return _manipulateUserStateValuesService.GetSelectedCategoryId();
    }
    
    public static void SetDefaultValues()
    {
        _manipulateUserStateValuesService.SetDefaultValues();
    }
    public static void SetSelectedUserId(long userId)
    {
        _manipulateUserStateValuesService.SetSelectedUserId(userId);
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