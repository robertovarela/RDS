namespace RDS.Api.Services;

public class UserService(
    UserManager<User> userManager,
    TokenServiceCore tokenServiceCore)
{
    public async Task<IList<string>> GetUserRolesAsync(User user)
    {
        return await userManager.GetRolesAsync(user);
    }
    
    public async Task<bool> VerifyIfIsAdmin(string token)
    {
        try
        {
            var userId = tokenServiceCore.GetUserIdFromToken(token, validateLifeTime: true);
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var roles = await userManager.GetRolesAsync(user);

            return roles.Contains("Admin", StringComparer.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }
}