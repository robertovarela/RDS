namespace RDS.Core.Services;

public class GetStateService()
{
    private static readonly UserStateService? userStateService;
    public static void Initialize(UserStateService userStateService)
    {
        userStateService.SetLoggedUserId(0);
        // userStateService.SelectedUserId = 0;
        // userStateService.SelectedUserAddressId = 0;
        // userStateService.SelectedCategoryId = 0;
    }

    public static long GetLoggedUserID()
    {
        //long loggedUserId = userStateService.LoggedUserId;
        return 0; //loggedUserId;
    }
}