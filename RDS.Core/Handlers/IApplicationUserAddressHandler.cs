namespace RDS.Core.Handlers;

public interface IApplicationUserAddressHandler
{
    Task<Response<ApplicationUserAddress?>> CreateAsync(CreateApplicationUserAddressRequest request);
    Task<Response<ApplicationUserAddress?>> UpdateAsync(UpdateApplicationUserAddressRequest request);
    Task<Response<ApplicationUserAddress?>> DeleteAsync(DeleteApplicationUserAddressRequest request);
    Task<Response<ApplicationUserAddress?>> GetByIdAsync(GetApplicationUserAddressByIdRequest request);
    Task<PagedResponse<List<ApplicationUserAddress>>> GetAllAsync(GetAllApplicationUserAddressRequest request);
}