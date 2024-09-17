using RDS.Core.Requests.Companies;

namespace RDS.Api.Handlers;

public class CompanyRequestUserRegistrationHandler(AppDbContext context) : ICompanyRequestUserRegistrationHandler
{
    public async Task<Response<CompanyRequestUserRegistration?>> CreateAsync(
        CreateCompanyRequestUserRegistrationRequest request)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var confirmationCode = GenerateConfirmationCodeFromGuid();
            var companyRequestUser = new CompanyRequestUserRegistration()
            {
                CompanyId = request.CompanyId,
                CompanyName = request.CompanyName,
                OwnerId = request.OwnerId,
                Email = request.Email,
                ConfirmationCode = confirmationCode,
                ExpirationDate = DateTime.UtcNow.AddDays(request.DaysForExpirationDate)
            };

            await context.CompaniesRequestUsersRegistration.AddAsync(companyRequestUser);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new Response<CompanyRequestUserRegistration?>(companyRequestUser, 201,
                "Requisição criada com sucesso!");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new Response<CompanyRequestUserRegistration?>(
                null, 500, "Não foi possível criar a requisição");
        }
    }

    public async Task<Response<CompanyRequestUserRegistration?>> UpdateAsync(
        UpdateCompanyRequestUserRegistrationRequest request)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var confirmationDate = DateTime.UtcNow;
            var companyRequestUser = await context
                .CompaniesRequestUsersRegistration
                .AsNoTracking()
                .FirstOrDefaultAsync(x
                    => x.ConfirmationCode == request.ConfirmationCode
                       && x.Email == request.Email
                       && x.ConfirmationDate == null);

            if (companyRequestUser is null)
                return new Response<CompanyRequestUserRegistration?>(
                    null, 404, "Requisição não encontrada");

            if (companyRequestUser.ExpirationDate > confirmationDate)
                companyRequestUser.ConfirmationDate = confirmationDate;

            context.CompaniesRequestUsersRegistration.Update(companyRequestUser);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new Response<CompanyRequestUserRegistration?>(
                companyRequestUser, 200, message: "Requisição atualizada com sucesso");
        }
        catch
        {
            await transaction.RollbackAsync();
            return new Response<CompanyRequestUserRegistration?>(
                null, 500, "Não foi possível alterar a requisição");
        }
    }

    public async Task<Response<CompanyRequestUserRegistration?>> DeleteAsync(
        DeleteCompanyRequestUserRegistrationRequest request)
    {
        if (!request.IsOwner)
            return new Response<CompanyRequestUserRegistration?>(
                null, 500, "Não foi possível excluir a requisição, verifique as permissões");

        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var companyRequestUser = await context
                .CompaniesRequestUsersRegistration
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (companyRequestUser is null)
                return new Response<CompanyRequestUserRegistration?>(
                    null, 404, "Requisição não encontrada");

            context.CompaniesRequestUsersRegistration.Remove(companyRequestUser);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new Response<CompanyRequestUserRegistration?>(
                companyRequestUser, message: "Requisição excluída com sucesso!");
        }
        catch
        {
            await transaction.RollbackAsync();
            return new Response<CompanyRequestUserRegistration?>(
                null, 500, "Não foi possível excluir a requisição");
        }
    }

    public async Task<PagedResponse<List<CompanyRequestUserRegistration>>> GetAllAsync(
        GetAllCompaniesRequestUserRegistrationRequest request)
    {
        try
        {
            var query = context.CompaniesRequestUsersRegistration.AsNoTracking();

            var companiesRequestUser = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                //.OrderBy(x => x.Id)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<CompanyRequestUserRegistration>>(
                companiesRequestUser,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<CompanyRequestUserRegistration>>
                (null, 500, "Não foi possível consultar as requisições");
        }
    }

    public async Task<Response<CompanyRequestUserRegistration?>> GetByUserEmailAsync(
        GetCompanyRequestUserRegistrationByUserEmailRequest request)
    {
        try
        {
            var companyRequestUser = await context
                .CompaniesRequestUsersRegistration
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CompanyId == request.CompanyId && x.Email == request.Email);

            return companyRequestUser is null
                ? new Response<CompanyRequestUserRegistration?>(null, 404, "Requisição não encontrada")
                : new Response<CompanyRequestUserRegistration?>(companyRequestUser, 200,
                    "Requisição solicitada com sucesso");
        }
        catch
        {
            return new Response<CompanyRequestUserRegistration?>
                (null, 500, "Não foi possível consultar as requisições");
        }
    }

    public async Task<Response<CompanyRequestUserRegistration?>> GetByConfirmationCodeAsync(
        GetCompanyRequestUserRegistrationByConfirmationCodeRequest request)
    {
        try
        {
            var companyRequestUser = await context
                .CompaniesRequestUsersRegistration
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ConfirmationCode == request.ConfirmationCode);

            return companyRequestUser is null
                ? new Response<CompanyRequestUserRegistration?>(
                    null, 404, "Requisição não encontrada")
                : new Response<CompanyRequestUserRegistration?>(companyRequestUser, 200,
                    "Requisição solicitada com sucesso");
        }
        catch
        {
            return new Response<CompanyRequestUserRegistration?>
                (null, 500, "Não foi possível consultar as requisições");
        }
    }

    private string GenerateConfirmationCodeFromGuid()
    {
        var guid = Guid.NewGuid().ToString("N").ToUpper()[7..32];

        const int separatorLength = 5;
        var segments = new StringBuilder(guid.Length + 4);

        for (int i = 0; i < guid.Length; i++)
        {
            if (i > 0 && i % separatorLength == 0)
            {
                segments.Append('-');
            }

            segments.Append(guid[i]);
        }

        return segments.ToString();
    }
}