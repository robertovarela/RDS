namespace RDS.Core.Handlers;

public interface ICompanyRequestUserRegistrationHandler
{
    Task<Response<CompanyRequestUserRegistration?>> CreateAsync(CreateCompanyRequestUserRegistrationRequest request);
    Task<Response<CompanyRequestUserRegistration?>> UpdateAsync(UpdateCompanyRequestUserRegistrationRequest request);
    Task<Response<CompanyRequestUserRegistration?>> DeleteAsync(DeleteCompanyRequestUserRegistrationRequest request); 
    Task<PagedResponse<List<CompanyRequestUserRegistration>>> GetAllAsync(GetAllCompaniesRequestUserRegistrationRequest request);
    Task<Response<CompanyRequestUserRegistration?>> GetByUserEmailAsync(GetCompanyRequestUserRegistrationByUserEmailRequest request); 
    Task<Response<CompanyRequestUserRegistration?>> GetByConfirmationCodeAsync(GetCompanyRequestUserRegistrationByConfirmationCodeRequest request); 
}