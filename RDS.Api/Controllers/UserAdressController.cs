namespace RDS.Api.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("v1/users/address")]
public class UserAdressController(
    AppDbContext context)
    : ControllerBase
{
    [HttpPost("createuser")]
    public async Task<Response<ApplicationUserAddress?>> CreateAsync([FromBody] CreateApplicationUserAddressRequest request)
    {
        try
        {
            var address = new ApplicationUserAddress
            {
                PostalCode = request.PostalCode,
                Street = request.Street,
                Number = request.Number,
                Complement = request.Complement,
                Neighborhood = request.Neighborhood,
                City = request.City,
                Country = request.Country,
                TypeOfAddress = request.TypeOfAddress,
                UserId = request.UserId
            };

            await context.Addresses.AddAsync(address);
            await context.SaveChangesAsync();

            return new Response<ApplicationUserAddress?>(address, 201, "Endereço criado com sucesso!");
        }
        catch
        {
            return new Response<ApplicationUserAddress?>(null, 500, "Não foi possível criar o endereço");
        }
    }

    [HttpPut("updateuser")]
    public async Task<Response<ApplicationUserAddress?>> UpdateAsync([FromBody] UpdateApplicationUserAddressRequest request)
    {
        try
        {
            var address = await context
                .Addresses
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (address is null)
                return new Response<ApplicationUserAddress?>(null, 404, "Endereço não encontrado");

            address.PostalCode = request.PostalCode;
            address.Street = request.Street;
            address.Number = request.Number;
            address.Complement = request.Complement;
            address.Neighborhood = request.Neighborhood;
            address.City = request.City;
            address.Country = request.Country;
            address.TypeOfAddress = request.TypeOfAddress;

            context.Addresses.Update(address);
            await context.SaveChangesAsync();

            return new Response<ApplicationUserAddress?>(address, message: "Endereço atualizado com sucesso");
        }
        catch
        {
            return new Response<ApplicationUserAddress?>(null, 500, "Não foi possível alterar o endereço");
        }
    }

    [HttpDelete("deleteuser")]
    public async Task<Response<ApplicationUserAddress?>> DeleteAsync([FromBody] DeleteApplicationUserAddressRequest request)
    {
        try
        {
            var address = await context
                .Addresses
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (address is null)
                return new Response<ApplicationUserAddress?>(null, 404, "Endereço não encontrado");

            context.Addresses.Remove(address);
            await context.SaveChangesAsync();

            return new Response<ApplicationUserAddress?>(address, message: "Endereço excluído com sucesso!");
        }
        catch
        {
            return new Response<ApplicationUserAddress?>(null, 500, "Não foi possível excluir o endereço");
        }
    }

    [HttpPost("allusers")]
    public async Task<PagedResponse<List<ApplicationUserAddress>>> GetAllAsync(
        [FromBody] GetAllApplicationUserAddressRequest request,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        try
        {
            var query = context
                .Addresses
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.TypeOfAddress)
                .ThenBy(x => x.Id);
            
            var address = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Cast<ApplicationUserAddress>()
                .ToListAsync();

            var count = await query.CountAsync();
            return count == 0
                ? new PagedResponse<List<ApplicationUserAddress>>(null, 404, "Usuário não encontrado")
                : new PagedResponse<List<ApplicationUserAddress>>(address, count, pageNumber, pageSize);
        }
        catch
        {
            return new PagedResponse<List<ApplicationUserAddress>>(null, 500, "Não foi possível consultar os usuários");
        }
    }

    [HttpPost("userbyid")]
    public async Task<Response<ApplicationUserAddress?>> GetByIdAsync([FromBody] GetApplicationUserAddressByIdRequest request)
    {
        try
        {
            var address = await context
                .Addresses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            return address is null
                ? new Response<ApplicationUserAddress?>(null, 404, "Endereço não encontrado")
                : new Response<ApplicationUserAddress?>(address);
        }
        catch
        {
            return new Response<ApplicationUserAddress?>(null, 500, "Não foi possível recuperar o endereço");
        }
    }
}