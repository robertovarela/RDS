#pragma warning disable CS8602 // Dereference of a possibly null reference.
namespace RDS.Web.Services;

public class StartService()
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

    public static async Task<long> GetSelectedUserId()
    {
        return await _manipulateUserStateValuesService?.GetSelectedUserId();
    }
}