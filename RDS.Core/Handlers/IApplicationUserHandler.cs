namespace RDS.Core.Handlers;

public interface IApplicationUserHandler
{
    Task<Response<UserLogin>> LoginAsync(LoginRequest? request);
    Task<Response<UserRefreshToken>> RefreshTokenAsync(RefreshTokenRequest? request);
    Task<Response<ApplicationUser?>> CreateAsync(CreateApplicationUserRequest request);
    Task<Response<ApplicationUser?>> UpdateAsync(UpdateApplicationUserRequest request);
    Task<Response<ApplicationUser?>> DeleteAsync(DeleteApplicationUserRequest request);
    Task<PagedResponse<List<AllUsersViewModel>>> GetAllAsync(GetAllApplicationUserRequest request);
    Task<PagedResponse<List<AllUsersViewModel>>> GetAllByCompanyIdAsync(GetAllApplicationUserRequest request);
    Task<Response<ApplicationUser?>> GetByIdAsync(GetApplicationUserByIdRequest request);
    Task<Response<ApplicationUser?>> GetByCpfAsync(GetApplicationUserByCpfRequest request);
    Task<PagedResponse<List<ApplicationUser>>> GetByNameAsync(GetApplicationUserByNameRequest request);
    Task<PagedResponse<List<ApplicationUser>>> GetByFullNameAsync(GetApplicationUserByFullNameRequest request);
    
}