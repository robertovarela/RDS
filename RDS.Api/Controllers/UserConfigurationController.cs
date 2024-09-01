namespace RDS.Api.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("v1/userconfiguration")]
public class UserConfigurationController(
        IApplicationUserConfigurationHandler applicationUserConfigurationHandler) : Controller
{
    [HttpPost("create-role")]
    public async Task<Response<ApplicationRole?>> CreateRole([FromBody] CreateApplicationRoleRequest request)
    {
        return await applicationUserConfigurationHandler.CreateRoleAsync(request);
    }

    [HttpDelete("delete-role")]
    public async Task<Response<ApplicationRole?>> DeleteRole([FromBody] DeleteApplicationRoleRequest request)
    {
        return await applicationUserConfigurationHandler.DeleteRoleAsync(request);
    }

    [HttpPost("list-roles")]
    public async Task<PagedResponse<List<ApplicationRole?>>> ListRoles()
    {
        return await applicationUserConfigurationHandler.ListRoleAsync();
    }

    [HttpPost("create-role-to-user")]
    public async Task<Response<ApplicationUserRole?>> CreateRoleToUser(CreateApplicationUserRoleRequest request)
    {
        return await applicationUserConfigurationHandler.CreateUserRoleAsync(request);
    }

    [HttpDelete("delete-role-to-user")]
    public async Task<Response<ApplicationUserRole?>> DeleteRoleToUser(DeleteApplicationUserRoleRequest request)
    {
        return await applicationUserConfigurationHandler.DeleteUserRoleAsync(request);
    }

    [HttpPost("list-roles-for-user")]
    public async Task<Response<List<ApplicationUserRole?>>> ListRolesForUser(GetAllApplicationUserRoleRequest request)
    {
        return await applicationUserConfigurationHandler.ListUserRoleAsync(request);
    }

    [HttpPost("list-roles-not-for-user")]
    public async Task<Response<List<ApplicationUserRole?>>> ListRolesNotForUser(GetAllApplicationUserRoleRequest request)
    {
        return await applicationUserConfigurationHandler.ListRoleToAddUserAsync(request);
    }
}