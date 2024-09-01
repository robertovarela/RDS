namespace RDS.Api.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("v1/users/address")]
public class UserAdressController(AppDbContext context, IApplicationUserAddressHandler applicationUserAddressHandler) : ControllerBase
{
    [HttpPost("createuseraddress")]
    public async Task<Response<ApplicationUserAddress?>> CreateAsync(
        [FromBody] CreateApplicationUserAddressRequest request)
    {
        return await applicationUserAddressHandler.CreateAsync(request);
    }

    [HttpPut("updateuseraddress")]
    public async Task<Response<ApplicationUserAddress?>> UpdateAsync(
        [FromBody] UpdateApplicationUserAddressRequest request)
    {
        return await applicationUserAddressHandler.UpdateAsync(request);
    }

    [HttpDelete("deleteuseraddress")]
    public async Task<Response<ApplicationUserAddress?>> DeleteAsync(
        [FromBody] DeleteApplicationUserAddressRequest request)
    {
        return await applicationUserAddressHandler.DeleteAsync(request);
    }

    [HttpPost("alluseraddresses")]
    public async Task<PagedResponse<List<ApplicationUserAddress>>> GetAllAsync(
        [FromBody] GetAllApplicationUserAddressRequest request)
    {
        return await applicationUserAddressHandler.GetAllAsync(request);
    }

    [HttpPost("useraddressbyid")]
    public async Task<Response<ApplicationUserAddress?>> GetByIdAsync(
        [FromBody] GetApplicationUserAddressByIdRequest request)
    {
        return await applicationUserAddressHandler.GetByIdAsync(request);
    }
}