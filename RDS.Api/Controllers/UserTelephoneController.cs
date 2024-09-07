namespace RDS.Api.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("v1/users/telephones")]
public class UserTelephoneController(IApplicationUserTelephoneHandler applicationUserTelephoneHandler) : ControllerBase
{
    [HttpPost("createusertelephone")]
    public async Task<Response<ApplicationUserTelephone?>> CreateAsync(
        [FromBody] CreateApplicationUserTelephoneRequest request)
    {
        return await applicationUserTelephoneHandler.CreateAsync(request);
    }

    [HttpPut("updateusertelephone")]
    public async Task<Response<ApplicationUserTelephone?>> UpdateAsync(
        [FromBody] UpdateApplicationUserTelephoneRequest request)
    {
        return await applicationUserTelephoneHandler.UpdateAsync(request);
    }

    [HttpDelete("deleteusertelephone")]
    public async Task<Response<ApplicationUserTelephone?>> DeleteAsync(
        [FromBody] DeleteApplicationUserTelephoneRequest request)
    {
        return await applicationUserTelephoneHandler.DeleteAsync(request);
    }

    [HttpPost("allusertelephones")]
    public async Task<PagedResponse<List<ApplicationUserTelephone>>> GetAllAsync(
        [FromBody] GetAllApplicationUserTelephoneRequest request)
    {
        return await applicationUserTelephoneHandler.GetAllAsync(request);
    }
    
    [HttpPost("usertelephonebyid")]
    public async Task<Response<ApplicationUserTelephone>> GetByIdAsync(
        [FromBody] GetApplicationUserTelephoneByIdRequest request)
    {
        return await applicationUserTelephoneHandler.GetByIdAsync(request);
    }
}