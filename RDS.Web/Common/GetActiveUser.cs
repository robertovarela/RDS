using MudBlazor;
using RDS.Core.Handlers;
using RDS.Core.Requests.ApplicationUsers;
using RDS.Core.Services;
using RDS.Web.Services;

namespace RDS.Web.Common;

public class GetActiveUser
{
    public static async Task GetUserIdAsync(ISnackbar snackbar, UserStateService userState, IApplicationUserHandler userHandler)
    {
        try
        {
            var request = new GetApplicationUserActiveRequest();
            var response = await userHandler.GetActiveAsync(request);
            if (response is { IsSuccess: true, Data: not null })
            {
                userState.SetLoggedUserId(response.Data.Id);
            }
        }
        catch (Exception ex)
        {
            snackbar.Add(ex.Message, Severity.Error);
        }
    }
}