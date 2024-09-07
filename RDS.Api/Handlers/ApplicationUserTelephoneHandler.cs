using RDS.Core.Requests.ApplicationUsers.Telephone;

namespace RDS.Api.Handlers;

public class ApplicationUserTelephoneHandler(AppDbContext context) : IApplicationUserTelephoneHandler
{
    public async Task<Response<ApplicationUserTelephone?>> CreateAsync(CreateApplicationUserTelephoneRequest request)
    {
        try
        {
            var telephone = new ApplicationUserTelephone
            {
                Number = request.Number,
                Type = request.Type,
                UserId = request.UserId,
            };

            await context.Telephones.AddAsync(telephone);
            await context.SaveChangesAsync();

            return new Response<ApplicationUserTelephone?>(telephone, 201, "Telefone criado com sucesso!");
        }
        catch (Exception ex)
        {
            return new Response<ApplicationUserTelephone?>(
                null, 500, $"{ex}\nNão foi possível criar o telefone");
        }
    }

    public async Task<Response<ApplicationUserTelephone?>> UpdateAsync(UpdateApplicationUserTelephoneRequest request)
    {
        try
        {
            var telephone = await context
                .Telephones
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (telephone is null)
                return new Response<ApplicationUserTelephone?>(null, 404, "Telefone não encontrado");

            telephone.Number = request.Number;
            telephone.Type = request.Type;

            context.Telephones.Update(telephone);
            await context.SaveChangesAsync();

            return new Response<ApplicationUserTelephone?>(telephone, message: "Telefone atualizado com sucesso");
        }
        catch
        {
            return new Response<ApplicationUserTelephone?>(
                null, 500, "Não foi possível alterar o telefone");
        }  
    }

    public async Task<Response<ApplicationUserTelephone?>> DeleteAsync(DeleteApplicationUserTelephoneRequest request)
    {
        try
        {
            var telephone = await context
                .Telephones
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (telephone is null)
                return new Response<ApplicationUserTelephone?>(null, 404, "Telefone não encontrado");

            context.Telephones.Remove(telephone);
            await context.SaveChangesAsync();

            return new Response<ApplicationUserTelephone?>(telephone, message: "Telefone excluído com sucesso!");
        }
        catch
        {
            return new Response<ApplicationUserTelephone?>(
                null, 500, "Não foi possível excluir o telefone");
        }   
    }

    public async Task<PagedResponse<List<ApplicationUserTelephone>>> GetAllAsync(GetAllApplicationUserTelephoneRequest request)
    {
        try
        {
            var query = context
                .Telephones
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.Number);

            var telephone = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Cast<ApplicationUserTelephone>()
                .ToListAsync();

            var count = await query.CountAsync();
            return count == 0
                ? new PagedResponse<List<ApplicationUserTelephone>>(
                    null, 404, "Usuário não encontrado")
                : new PagedResponse<List<ApplicationUserTelephone>>(telephone, count, request.PageNumber, request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<ApplicationUserTelephone>>(
                null, 500, "Não foi possível consultar os usuários");
        }
    }
}