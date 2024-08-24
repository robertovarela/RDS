using RDS.Core.Models.Company;
using RDS.Core.Models.ViewModels.company;
using RDS.Core.Requests.Companies;
using UpdateCompanyRequest = RDS.Core.Requests.Companies.UpdateCompanyRequest;

namespace RDS.Api.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("v1/companies")]
public class CompanyController(AppDbContext context) : ControllerBase
{
    [HttpPost("create")]
    public async Task<Response<Company?>> CreateAsync([FromBody] CreateCompanyRequest request)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var company = new Company
            {
                Name = request.Name,
                Description = request.Description,
                OwnerId = request.OwnerId
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();

            var companyUser = new CompanyUser
            {
                CompanyId = company.Id,
                UserId = request.OwnerId,
                IsAdmin = true
            };

            await context.CompanyUsers.AddAsync(companyUser);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new Response<Company?>(company, 201, "Empresa criada com sucesso!");
        }
        catch
        {
            await transaction.RollbackAsync();
            return new Response<Company?>(null, 500, "Não foi possível criar a empresa");
        }
    }

    [HttpPost("create-user")]
    public async Task<Response<CompanyUser?>> CreateUserAsync([FromBody] CreateCompanyUserRequest request)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var companyUser = new CompanyUser
            {
                CompanyId = request.CompanyId,
                UserId = request.OwnerId,
                IsAdmin = request.IsAdmin
            };

            await context.CompanyUsers.AddAsync(companyUser);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new Response<CompanyUser?>
                (companyUser, 201, "Usuário da empresa criado com sucesso!");
        }
        catch
        {
            await transaction.RollbackAsync();
            return new Response<CompanyUser?>(null, 500, "Não foi possível criar o usuário");
        }
    }

    [HttpPut("update")]
    public async Task<Response<Company?>> UpdateAsync([FromBody] UpdateCompanyRequest request)
    {
        try
        {
            var company = await context
                .Companies
                .FirstOrDefaultAsync(x => x.Id == request.CompanyId);

            if (company is null)
                return new Response<Company?>(null, 404, "Empresa não encontrada");

            company.Name = request.Name;
            company.Description = request.Description;

            context.Companies.Update(company);
            await context.SaveChangesAsync();

            return new Response<Company?>(company, message: "Empresa atualizada com sucesso");
        }
        catch
        {
            return new Response<Company?>(null, 500, "Não foi possível alterar a empresa");
        }
    }

    [HttpDelete("delete")]
    public async Task<Response<Company?>> DeleteAsync([FromBody] DeleteCompanyRequest request)
    {
        try
        {
            var company = await context
                .Companies
                .FirstOrDefaultAsync(x => x.Id == request.CompanyId);

            if (company is null)
                return new Response<Company?>(null, 404, "Empresa não encontrada");

            context.Companies.Remove(company);
            await context.SaveChangesAsync();

            return new Response<Company?>(company, message: "Empresa excluída com sucesso!");
        }
        catch
        {
            return new Response<Company?>(null, 500, "Não foi possível excluir a empresa");
        }
    }

    [HttpPost("all")]
    public async Task<PagedResponse<List<Company>>> GetAllAsync([FromBody] GetAllCompaniesRequest request)
    {
        try
        {
            var query = context
                .Companies
                .AsNoTracking()
                //.Where(x => x.Id == request.CompanyId)
                .OrderBy(x => x.Name);

            var companies = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Company>>(
                companies,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Company>>
                (null, 500, "Não foi possível consultar as empresas");
        }
    }

    [HttpPost("allbyuserid")]
    public async Task<PagedResponse<List<Company>>> GetAllByUserIdAsync([FromBody] GetAllCompaniesByUserIdRequest request)
    {
        try
        {
            var query = context
                .Companies
                .AsNoTracking()
                .Where(x => x.OwnerId == request.UserId)
                .OrderBy(x => x.Name);

            var companies = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Company>>(
                companies,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Company>>
                (null, 500, "Não foi possível consultar as empresas");
        }
    }
    
    [HttpPost("allcompanyidbyuserid")]
    public async Task<PagedResponse<List<AllCompaniesIdViewModel>>> GetAllCompanyIdByUserIdAsync([FromBody] GetAllCompaniesByUserIdRequest request)
    {
        try
        {
            var query = context
                .Companies
                .AsNoTracking()
                .Where(x => x.OwnerId == request.UserId)
                .Select(x => new AllCompaniesIdViewModel { CompanyId = x.Id})
                .OrderBy(x => x.CompanyId);

            var companies = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<AllCompaniesIdViewModel>>(
                companies,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<AllCompaniesIdViewModel>>
                (null, 500, "Não foi possível consultar as empresas");
        }
    }
    
    [HttpPost("byid")]
    public async Task<Response<Company?>> GetByIdAsync([FromBody] GetCompanyByIdRequest request)
    {
        try
        {
            var company = await context
                .Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.CompanyId);

            return company is null
                ? new Response<Company?>(null, 404, "companhia não encontrada")
                : new Response<Company?>(company);
        }
        catch
        {
            return new Response<Company?>(null, 500, "Não foi possível recuperar a companhia");
        }
    }
}