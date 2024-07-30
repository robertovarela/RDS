namespace RDS.Core.Handlers;

public interface IApplicationUserHandler
{
    Task<Response<ApplicationUser?>> CreateAsync(CreateApplicationUserRequest request);
    Task<Response<ApplicationUser?>> UpdateAsync(UpdateApplicationUserRequest request);
    Task<Response<ApplicationUser?>> DeleteAsync(DeleteApplicationUserRequest request);
    Task<Response<ApplicationUser?>> GetByIdAsync(GetApplicationUserByIdRequest request);
    Task<Response<ApplicationUser?>> GetActiveAsync(GetApplicationUserActiveRequest request);
    Task<Response<ApplicationUser?>> GetByCpfAsync(GetApplicationUserByCpfRequest request);
    Task<PagedResponse<List<ApplicationUser>>> GetByNameAsync(GetApplicationUserByNameRequest request);
    Task<PagedResponse<List<ApplicationUser>>> GetByFullNameAsync(GetApplicationUserByFullNameRequest request);
    Task<PagedResponse<List<ApplicationUser>>> GetAllAsync(GetAllApplicationUserRequest request);
}