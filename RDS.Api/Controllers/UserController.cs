using RDS.Core.Models.ViewModels.ApplicationUser;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
namespace RDS.Api.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("v1/users")]
public class UserController(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    //RoleManager<IdentityRole<long>> roleManager,
    JwtTokenService jwtTokenService,
    ILogger<UserController> logger,
    AppDbContext context,
    IApplicationUserHandler applicationUserHandler)
    : ControllerBase
{
    [HttpPost("userlogin")]
    public async Task<Response<UserLogin>> LoginAsync([FromBody] LoginRequest? request)
    {
        return await applicationUserHandler.LoginAsync(request);
    }

    [HttpPost("refreshtoken")]
    public async Task<Response<UserRefreshToken>> RefreshTokenAsync([FromBody] RefreshTokenRequest? request)
    {
        return await applicationUserHandler.RefreshTokenAsync(request);
    }

    [HttpPost("createuser")]
    public async Task<Response<ApplicationUser?>> CreateAsync([FromBody] CreateApplicationUserRequest request)
    {
        return await applicationUserHandler.CreateAsync(request);
    }

    [HttpPut("updateuser")]
    public async Task<Response<ApplicationUser?>> UpdateAsync([FromBody] UpdateApplicationUserRequest request)
    {
        return await applicationUserHandler.UpdateAsync(request);
    }

    [HttpDelete("deleteuser")]
    public async Task<Response<ApplicationUser?>> DeleteAsync([FromBody] DeleteApplicationUserRequest request)
    {
        return await applicationUserHandler.DeleteAsync(request);
    }

    [HttpPost("allusers")]
    public async Task<PagedResponse<List<AllUsersViewModel>>> GetAllAsync(
        [FromBody] GetAllApplicationUserRequest request)
    {
        return await applicationUserHandler.GetAllAsync(request);
    }

    [HttpPost("allusersbycompanyid")]
    public async Task<PagedResponse<List<AllUsersViewModel>>> GetAllByCompanyIdAsync(
        [FromBody] GetAllApplicationUserRequest request)
    {
        return await applicationUserHandler.GetAllByCompanyIdAsync(request);
    }


    [HttpPost("allusers2")]
    public async Task<PagedResponse<List<ApplicationUser>>> GetAll2Async(
        [FromBody] GetAllApplicationUserRequest request)
    {
        try
        {
            var query = context
                .Users
                .AsNoTracking()
                .Where(u =>
                    (string.IsNullOrEmpty(request.Filter) || u.Name.Contains(request.Filter)) ||
                    (string.IsNullOrEmpty(request.Filter) || u.Cpf.Contains(request.Filter)))
                .OrderBy(u => u.Name)
                .ThenBy(u => u.Id);

            var users = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Cast<ApplicationUser>()
                .ToListAsync();

            var count = await query.CountAsync();
            return count == 0
                ? new PagedResponse<List<ApplicationUser>>(null, 404, "Usuário não encontrado")
                : new PagedResponse<List<ApplicationUser>>(users, count, request.PageNumber, request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<ApplicationUser>>(null, 500, "Não foi possível consultar os usuários");
        }
    }

    [HttpPost("userbyid")]
    public async Task<Response<ApplicationUser?>> GetByIdAsync([FromBody] GetApplicationUserByIdRequest request)
    {
        return await applicationUserHandler.GetByIdAsync(request);
    }

    [HttpPost("userbycpf")]
    public async Task<Response<ApplicationUser?>> GetByCpfAsync([FromBody] GetApplicationUserByCpfRequest request)
    {
        return await applicationUserHandler.GetByCpfAsync(request);
    }

    [HttpPost("userbyname")]
    public async Task<PagedResponse<List<ApplicationUser>>> GetByNameAsync(
        [FromBody] GetApplicationUserByNameRequest request)
    {
       return await applicationUserHandler.GetByNameAsync(request);
    }

    [HttpPost("userbyfullname")]
    public async Task<PagedResponse<List<ApplicationUser>>> GetByFullNameAsync(
        [FromBody] GetApplicationUserByFullNameRequest request)
    {
        return await applicationUserHandler.GetByFullNameAsync(request);
    }
}