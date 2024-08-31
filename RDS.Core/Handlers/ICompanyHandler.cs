﻿using RDS.Core.Models.Company;
using RDS.Core.Models.ViewModels.company;
using UpdateCompanyRequest = RDS.Core.Requests.Companies.UpdateCompanyRequest;

namespace RDS.Core.Handlers;

public interface ICompanyHandler
{
    Task<Response<Company?>> CreateAsync(CreateCompanyRequest request);
    Task<Response<Company?>> UpdateAsync(UpdateCompanyRequest request);
    Task<Response<Company?>> DeleteAsync(DeleteCompanyRequest request);//  
    Task<Response<List<Company>>> GetAllAsync(GetAllCompaniesRequest request);
    Task<Response<List<Company>>> GetAllByUserIdAsync(GetAllCompaniesByUserIdRequest request);
    Task<PagedResponse<List<CompanyIdNameViewModel>>> GetAllCompanyIdNameByAdminAsync(GetAllCompaniesByUserIdRequest request);
    Task<PagedResponse<List<CompanyIdNameViewModel>>> GetAllCompanyIdNameByUserIdAsync(GetAllCompaniesByUserIdRequest request);
    Task<Response<Company?>> GetByIdAsync(GetCompanyByIdRequest request);
    
}