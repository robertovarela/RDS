using UpdateCompanyRequest = RDS.Core.Requests.Companies.UpdateCompanyRequest;

namespace RDS.Api.Controllers;

[ApiController]
[Route("v1/companies")]
public class CompanyController(ICompanyHandler companyHandler) : ControllerBase
{
    [HttpPost("create")]
    public async Task<Response<Company?>> CreateAsync([FromBody] CreateCompanyRequest request)
    {
        return await companyHandler.CreateAsync(request);
    }

    [HttpPost("create-user")]
    public async Task<Response<CompanyUser?>> CreateUserAsync([FromBody] CreateCompanyUserRequest request)
    {
        return await companyHandler.CreateUserAsync(request);
    }

    [HttpPut("update")]
    public async Task<Response<Company?>> UpdateAsync([FromBody] UpdateCompanyRequest request)
    {
        return await companyHandler.UpdateAsync(request);
    }

    [HttpDelete("delete")]
    public async Task<Response<Company?>> DeleteAsync([FromBody] DeleteCompanyRequest request)
    {
        return await companyHandler.DeleteAsync(request);
    }

    [HttpPost("all")]
    public async Task<PagedResponse<List<Company>>> GetAllAsync([FromBody] GetAllCompaniesRequest request)
    {
        return await companyHandler.GetAllAsync(request);
    }

    [HttpPost("allbyuserid")]
    public async Task<PagedResponse<List<Company>>> GetAllByUserIdAsync(
        [FromBody] GetAllCompaniesByUserIdRequest request)
    {
        return await companyHandler.GetAllByUserIdAsync(request);
    }

    [HttpPost("allcompanyidnamebyrole")]
    public async Task<PagedResponse<List<CompanyIdNameViewModel>>> GetAllCompanyIdNameByRoleAsync(
        [FromBody] GetAllCompaniesByUserIdRequest request)
    {
        return await companyHandler.GetAllCompanyIdNameByRoleAsync(request);
    }
    
    [HttpPost("byid")]
    public async Task<Response<Company?>> GetByIdAsync([FromBody] GetCompanyByIdRequest request)
    {
        return await companyHandler.GetByIdAsync(request);
    }
}