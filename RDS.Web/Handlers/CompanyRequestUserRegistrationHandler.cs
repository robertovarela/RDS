namespace RDS.Web.Handlers;

public class CompanyRequestUserRegistrationHandler(HttpClientService httpClientService)
    : ICompanyRequestUserRegistrationHandler
{
    public async Task<Response<CompanyRequestUserRegistration?>> CreateAsync(CreateCompanyRequestUserRegistrationRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<CompanyRequestUserRegistration?>> UpdateAsync(UpdateCompanyRequestUserRegistrationRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<CompanyRequestUserRegistration?>> DeleteAsync(DeleteCompanyRequestUserRegistrationRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResponse<List<CompanyRequestUserRegistration>>> GetAllAsync(GetAllCompaniesRequestUserRegistrationRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<CompanyRequestUserRegistration?>> GetByUserEmailAsync(GetCompanyRequestUserRegistrationByUserEmailRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<CompanyRequestUserRegistration?>> GetByConfirmationCodeAsync(GetCompanyRequestUserRegistrationByConfirmationCodeRequest request)
    {
        throw new NotImplementedException();
    }
}