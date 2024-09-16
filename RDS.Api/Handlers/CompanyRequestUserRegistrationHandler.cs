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
            var confirmationCode = GenerateConfirmationCode();
            var companyRequestUser = new CompanyRequestUserRegistration()
            {
                CompanyId = request.CompanyId,
                CompanyName = request.CompanyName,
                OwnerId = request.OwnerId,
                Email = request.Email,
                ConfirmationCode = confirmationCode,
                ConfirmationDate = DateTime.Now,
            };

            await context.CompaniesRequestUsersRegistration.AddAsync(companyRequestUser);
            await context.SaveChangesAsync();

            var companyUser = new CompanyUser
            {
                CompanyId = companyRequestUser.Id,
                UserId = request.OwnerId,
                IsAdmin = true
            };

            await context.CompanyUsers.AddAsync(companyUser);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new Response<CompanyRequestUserRegistration?>(companyRequestUser, 201,
                "Requisição criada com sucesso!");
        }
        catch
        {
            await transaction.RollbackAsync();
            return new Response<CompanyRequestUserRegistration?>(null, 500, "Não foi possível criar a requisição");
        }
    }

    public async Task<Response<CompanyRequestUserRegistration?>> DeleteAsync(
        DeleteCompanyRequestUserRegistrationRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResponse<List<CompanyRequestUserRegistration?>>> GetAllAsync(
        GetAllCompaniesRequestUserRegistrationRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<CompanyRequestUserRegistration?>> GetCompanyRequestUserRegistrationByUserEmailAsync(
        GetCompanyRequestUserRegistrationByUserEmailRequest request)
    {
        throw new NotImplementedException();
    }

    private string GenerateConfirmationCode()
    {
        const int totalLength = 25;
        const int segmentLength = 5;
        const int numSegments = totalLength / segmentLength;
        var random = new Random();
        var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var confirmationCodeBuilder =
            new StringBuilder(totalLength + numSegments - 1);

        for (int i = 0; i < numSegments; i++)
        {
            for (int j = 0; j < segmentLength; j++)
            {
                confirmationCodeBuilder.Append(characters[random.Next(characters.Length)]);
            }

            if (i < numSegments - 1)
            {
                confirmationCodeBuilder.Append('-');
            }
        }

        return confirmationCodeBuilder.ToString();
    }
    
    private string GenerateConfirmationCodeFromGuid()
    {
        var guid = Guid.NewGuid().ToString("N").ToUpper()[7..32];

        const int totalLength = 25;
        const int segmentLength = 5;
        var result = new StringBuilder(totalLength + totalLength / segmentLength - 1);
        for (int i = 0; i < totalLength; i++)
        {
            if (i > 0 && i % 5 == 0)
                result.Append('-');

            result.Append(characters[data[i] % characters.Length]);
        }
        
        return result.ToString();
        
    }


    private string GenerateSecureConfirmationCode(int length = 25)
    {
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var data = new byte[length];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(data);
        }

        var result = new StringBuilder(length + length / 5 - 1);
        for (int i = 0; i < length; i++)
        {
            if (i > 0 && i % 5 == 0)
                result.Append('-');

            result.Append(characters[data[i] % characters.Length]);
        }

        return result.ToString();
    }
}