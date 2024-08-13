namespace RDS.Core.Handlers;

public interface IApplicationUserConfigurationHandler
{
    Task<Response<ApplicationRole?>> CreateRoleAsync(CreateApplicationRoleRequest request);
    Task<Response<ApplicationRole?>> DeleteRoleAsync(DeleteApplicationRoleRequest request);
    Task<Response<ApplicationRole?>> ListRoleAsync();
    Task<Response<ApplicationUserRole?>> CreateUserRoleAsync(CreateApplicationUserRoleRequest request);
    Task<Response<ApplicationUserRole?>> DeleteUserRoleAsync(DeleteApplicationUserRoleRequest request);
    Task<Response<ApplicationUserRole?>> ListUserRoleAsync(GetApplicationUserByIdRequest request);
}