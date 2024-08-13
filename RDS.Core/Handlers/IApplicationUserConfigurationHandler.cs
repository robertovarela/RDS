namespace RDS.Core.Handlers;

public interface IApplicationUserConfigurationHandler
{
    Task<Response<ApplicationRole?>> CreateRoleAsync(CreateApplicationRoleRequest request);
    Task<Response<ApplicationRole?>> DeleteRoleAsync(DeleteApplicationRoleRequest request);
    Task<Response<ApplicationUserRole?>> CreateUserRoleAsync(CreateApplicationUserRoleRequest request);
    Task<Response<ApplicationUserRole?>> DeleteUserRoleAsync(DeleteApplicationUserRoleRequest request);
}