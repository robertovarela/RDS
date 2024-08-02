namespace RDS.Api.Middlewares;

public class UserStateMiddleware
{
    private readonly RequestDelegate _next;

    public UserStateMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // public async Task InvokeAsync(HttpContext context, UserStateService userStateService)
    // {
    //     var user = context.User;
    //
    //     // Configure o UserStateService com base nos claims do usuário autenticado
    //     var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //     if (long.TryParse(userIdClaim, out var userId))
    //     {
    //         userStateService.SetLoggedUserId(userId);
    //     }
    //
    //     // Verifique e configure outros claims conforme necessário
    //     var currentUserIdClaim = user.FindFirst("CurrentUserId")?.Value;
    //     if (long.TryParse(currentUserIdClaim, out var currentUserId))
    //     {
    //         userStateService.SelectedUserId = currentUserId;
    //     }
    //
    //     var currentUserAddressIdClaim = user.FindFirst("CurrentUserAddressId")?.Value;
    //     if (long.TryParse(currentUserAddressIdClaim, out var currentUserAddressId))
    //     {
    //         userStateService.SelectedUserAddressId = currentUserAddressId;
    //     }
    //
    //     var currentCategoryIdClaim = user.FindFirst("CurrentCategoryId")?.Value;
    //     if (long.TryParse(currentCategoryIdClaim, out var currentCategoryId))
    //     {
    //         userStateService.SelectedCategoryId = currentCategoryId;
    //     }
    //
    //     await _next(context);
    // }
}