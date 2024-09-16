using RDS.Core.Requests.Companies;

namespace RDS.Api.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("v1/companies-request-user-registration")]
public class CompanyRequestUserRegistrationController(ICompanyRequestUserRegistrationHandler handler) : ControllerBase
{
    [HttpPost]
    [Route("create")]
    public async Task<Response<CompanyRequestUserRegistration?>> CreateAsync(
        [FromBody] CreateCompanyRequestUserRegistrationRequest request)
    {
        return await handler.CreateAsync(request);
    }

    [HttpPut]
    [Route("update")]
    public async Task<Response<CompanyRequestUserRegistration?>>  UpdateAsync(
        [FromBody] UpdateCompanyRequestUserRegistrationRequest request)
    {
        return await handler.UpdateAsync(request);
    }
    
    [HttpDelete]
    [Route("delete")]
    public async Task<Response<CompanyRequestUserRegistration?>>  DeleteAsync(
        [FromBody] DeleteCompanyRequestUserRegistrationRequest request)
    {
        return await handler.DeleteAsync(request);
    }

    [HttpPost]
    [Route("get-all")]
    public async Task<PagedResponse<List<CompanyRequestUserRegistration?>>> GetAllAsync(
        [FromBody] GetAllCompaniesRequestUserRegistrationRequest request)
    {
        return await handler.GetAllAsync(request);
    }

    [HttpPost]
    [Route("get-by-email")]
    public async Task<Response<CompanyRequestUserRegistration?>> GetByEmailAsync(
        [FromBody] GetCompanyRequestUserRegistrationByUserEmailRequest request)
    {
        return await handler.GetCompanyRequestUserRegistrationByUserEmailAsync(request);
    }
}