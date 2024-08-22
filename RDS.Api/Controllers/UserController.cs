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
    AppDbContext context)
    : ControllerBase
{
    [HttpPost("userlogin")]
    public async Task<Response<UserLogin>> LoginAsync([FromBody] LoginRequest? request)
    {
        if (request is null)
        {
            return new Response<UserLogin>(null, 400, "Dados inválidos");
        }

        try
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return new Response<UserLogin>(null, 401, "Usuário não encontrado");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            logger.LogInformation("SignIn result: {Result}", result);

            if (!result.Succeeded)
            {
                return new Response<UserLogin>(null, 401, "Credenciais inválidas");
            }

            var roles = await userManager.GetRolesAsync(user);
            var token = jwtTokenService.GenerateToken(user, roles, request.FingerPrint);
            var response = new UserLogin(request.Email, token);

            return new Response<UserLogin>(response, 200, "Login realizado com sucesso");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during the login process.");
            return new Response<UserLogin>(null, 500, "Erro interno no servidor");
        }
    }

    [HttpPost("refreshtoken")]
    public async Task<Response<UserRefreshToken>> RefreshTokenAsync([FromBody] RefreshTokenRequest? request)
    {
        if (request == null)
        {
            return new Response<UserRefreshToken>(null, 400, "Dados inválidos");
        }

        try
        {
            var refreshToken = await jwtTokenService.RenewTokenIfNecessary(request);
            var response = new UserRefreshToken(refreshToken);

            return new Response<UserRefreshToken>(response, 200, "Refresh Token efetuado com sucesso");
        }
        catch
        {
            return new Response<UserRefreshToken>(null, 500, "Erro interno no servidor");
        }
    }

    [HttpPost("createuser")]
    public async Task<Response<ApplicationUser?>> CreateAsync([FromBody] CreateApplicationUserRequest request)
    {
        try
        {
            if (request.RepeatPassword != request.Password)
            {
                return new Response<ApplicationUser?>(null, 400, "As senhas digitadas não são iguais");
            }

            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                Name = request.Name,
                CreateAt = DateTime.Now,
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new Response<ApplicationUser?>(null, 400, "Não foi possível criar o usuário");
            }

            const string roleName = "User";
            var resultRole = await userManager.AddToRoleAsync(user, roleName);

            return resultRole.Succeeded
                ? new Response<ApplicationUser?>(user, 201, "Usuário criado com sucesso!")
                : new Response<ApplicationUser?>(null, 400, "Não foi possível criar o usuário");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating the user.");
            return new Response<ApplicationUser?>(null, 500, "Erro interno no servidor");
        }
    }

    [HttpPut("updateuser")]
    public async Task<Response<ApplicationUser?>> UpdateAsync([FromBody] UpdateApplicationUserRequest request)
    {
        try
        {
            var user = await userManager.FindByIdAsync(request.CompanyId.ToString());

            if (user == null)
            {
                return new Response<ApplicationUser?>(null, 404, "Usuário não encontrado");
            }

            user.Name = request.Name;
            user.Email = request.Email.Trim().ToLower();
            user.NormalizedEmail = user.Email.ToUpper();
            user.UserName = user.Email;
            user.NormalizedUserName = user.NormalizedEmail;
            user.Cpf = request.Cpf;
            user.BirthDate = request.BirthDate;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
                return new Response<ApplicationUser?>(user, message: "Usuário atualizado com sucesso!");

            return new Response<ApplicationUser?>(null, 400, "Não foi possível atualizar o usuário");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating the user.");
            return new Response<ApplicationUser?>(null, 500, "Erro interno no servidor");
        }
    }

    [HttpDelete("deleteuser")]
    public async Task<Response<ApplicationUser?>> DeleteAsync([FromBody] DeleteApplicationUserRequest request)
    {
        try
        {
            var user = await userManager.FindByIdAsync(request.CompanyId.ToString());

            if (user == null)
            {
                return new Response<ApplicationUser?>(null, 404, "Usuário não encontrado");
            }

            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return new Response<ApplicationUser?>(null, 400, "Não foi possível excluir o usuário");
            }

            return new Response<ApplicationUser?>(user, message: "Usuário excluído com sucesso!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting the user.");
            return new Response<ApplicationUser?>(null, 500, "Erro interno no servidor");
        }
    }

    [HttpPost("allusers")]
    public async Task<PagedResponse<List<ListAllUsers>>> GetAllAsync(
        [FromBody] GetAllApplicationUserRequest request)
    {
        try
        {
            var notFoundMessage = "";
            long.TryParse(request.Filter, out long filterId);
            IQueryable<ListAllUsers> query;

            if (filterId != 0)
            {
                if (request.Filter.Length == 11)
                {
                    query = context.Users.AsNoTracking()
                        .Where(u => u.Cpf == request.Filter)
                        .Select(u => new ListAllUsers { Id = u.Id, Name = u.Name, Email = u.Email! });

                    notFoundMessage = "CPF não localizado";
                }
                else
                {
                    query = context.Users.AsNoTracking()
                        .Where(u => u.Id == filterId)
                        .Select(u => new ListAllUsers { Id = u.Id, Name = u.Name, Email = u.Email! });

                    notFoundMessage = "Código não localizado";
                }
            }
            else
            {
                query = context.Users.AsNoTracking()
                    .Where(u =>
                        (string.IsNullOrEmpty(request.Filter) || u.Name.StartsWith(request.Filter)) ||
                        (string.IsNullOrEmpty(request.Filter) || u.Email == request.Filter))
                    .OrderBy(u => u.Id)
                    .Select(u => new ListAllUsers { Id = u.Id, Name = u.Name!, Email = u.Email! });

                notFoundMessage = request.Filter.Contains("@") ? "Email não localizado" : "Usuário não encontrado";
            }

            var users = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .OrderBy(users => users.Name)
                .ToListAsync();

            var count = await query.CountAsync();
            return count == 0
                ? new PagedResponse<List<ListAllUsers>>(null, 404, notFoundMessage)
                : new PagedResponse<List<ListAllUsers>>(users, count, request.PageNumber, request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<ListAllUsers>>(null, 500, "Não foi possível consultar os usuários");
        }
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
        try
        {
            var user = await userManager.FindByIdAsync(request.CompanyId.ToString());

            if (user == null)
            {
                return new Response<ApplicationUser?>(null, 404, "Usuário não encontrado");
            }

            return new Response<ApplicationUser?>(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting the user.");
            return new Response<ApplicationUser?>(null, 500, "Não foi possível recuperar o usuário");
        }
    }

    [HttpPost("userbycpf")]
    public async Task<Response<ApplicationUser?>> GetByCpfAsync([FromBody] GetApplicationUserByCpfRequest request)
    {
        try
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Cpf == request.Cpf);

            if (user == null)
            {
                return new Response<ApplicationUser?>(null, 404, "Usuário não encontrado");
            }

            return new Response<ApplicationUser?>(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting the user.");
            return new Response<ApplicationUser?>(null, 500, "Não foi possível recuperar o usuário");
        }
    }

    [HttpPost("userbyname")]
    public async Task<PagedResponse<List<ApplicationUser>>> GetByNameAsync(
        [FromBody] GetApplicationUserByNameRequest request,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        try
        {
            var query = context
                .Users
                .AsNoTracking()
                .Where(u => EF.Functions.Like(u.UserName, $"%{request.UserName}%"))
                .OrderBy(u => u.Name)
                .Cast<ApplicationUser>();

            var users = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return count == 0
                ? new PagedResponse<List<ApplicationUser>>(null, 404, "Usuário não encontrado")
                : new PagedResponse<List<ApplicationUser>>(users, count, pageNumber, pageSize);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting the user.");
            return new PagedResponse<List<ApplicationUser>>(null, 500, "Não foi possível recuperar o usuário");
        }
    }

    [HttpPost("userbyfullname")]
    public async Task<PagedResponse<List<ApplicationUser>>> GetByFullNameAsync(
        [FromBody] GetApplicationUserByFullNameRequest request,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        try
        {
            var query = context
                .Users
                .AsNoTracking()
                .Where(u => EF.Functions.Like(u.UserName, $"{request.UserName}%"))
                .OrderBy(u => u.Name)
                .Cast<ApplicationUser>();

            var users = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return count == 0
                ? new PagedResponse<List<ApplicationUser>>(null, 404, "Usuário não encontrado")
                : new PagedResponse<List<ApplicationUser>>(users, count, pageNumber, pageSize);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting the user.");
            return new PagedResponse<List<ApplicationUser>>(null, 500, "Não foi possível recuperar o usuário");
        }
    }
}