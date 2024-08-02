#pragma warning disable CS8602 // Dereference of a possibly null reference.
namespace RDS.Web.Services;

public abstract class StartService()
{
    private static ManipulateUserStateValuesService? _manipulateUserStateValuesService;
    public static void Initialize(ManipulateUserStateValuesService manipulateUserStateValuesService)
    {
        _manipulateUserStateValuesService = manipulateUserStateValuesService;
    }
    
    public static async Task<long> SetDefaultValues()
    {
        return await _manipulateUserStateValuesService?.SetDefaultValues();
    }

    public static async Task<long> GetLoggedUserId()
    {
        return await _manipulateUserStateValuesService.GetLoggedUserId();
    }
    public static async Task<long> GetSelectedUserId()
    {
        return await _manipulateUserStateValuesService?.GetSelectedUserId();
    }
    
    public static async Task<long> GetSelectedAddressId()
    {
        return await _manipulateUserStateValuesService?.GetSelectedAddressId();
    }
    
    public static async Task<long> GetSelectedCategoriId()
    {
        return await _manipulateUserStateValuesService?.GetSelectedCategoryId();
    }
}