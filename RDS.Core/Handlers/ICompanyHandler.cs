using RDS.Core.Models.Company;
using UpdateCompanyRequest = RDS.Core.Requests.Companies.UpdateCompanyRequest;

namespace RDS.Core.Handlers;

public interface ICompanyHandler
{
    Task<Response<Company?>> CreateAsync(CreateCompanyRequest request);
    Task<Response<Company?>> UpdateAsync(UpdateCompanyRequest request);
    Task<Response<Company?>> DeleteAsync(DeleteCompanyRequest request);
    Task<Response<Company?>> GetByIdAsync(GetCompanyByIdRequest request);
    Task<PagedResponse<List<Company>>> GetAllAsync(GetAllCompaniesRequest request);
}