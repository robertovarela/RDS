using RDS.Core.Requests.ApplicationUsers.Telephone;

namespace RDS.Core.Handlers;

public interface IApplicationUserTelephoneHandler
{
    Task<Response<ApplicationUserTelephone?>> CreateAsync(CreateApplicationUserTelephoneRequest request);
    Task<Response<ApplicationUserTelephone?>> UpdateAsync(UpdateApplicationUserTelephoneRequest request);
    Task<Response<ApplicationUserTelephone?>> DeleteAsync(DeleteApplicationUserTelephoneRequest request);
    Task<PagedResponse<List<ApplicationUserTelephone>>> GetAllAsync(GetAllApplicationUserTelephoneRequest request);
}