namespace RDS.Api.Controllers;

[ApiController]
[Microsoft.AspNetCore.Components.Route("v1/useridentity")]
public class UserIdentityController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly JwtTokenService _jwtTokenService;
    private readonly ILogger<UserIdentityController> _logger;
    private readonly AppDbContext _context;

    public UserIdentityController(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        JwtTokenService jwtTokenService,
        ILogger<UserIdentityController> logger,
        AppDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
        _context = context;
    }

    [HttpPost("userlogin")]
    public async Task<Response<UserLogin>> LoginAsync([FromBody] LoginRequest? request)
    {
        if (request == null)
        {
            return new Response<UserLogin>(null, 400, "Dados inválidos");
        }

        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new Response<UserLogin>(null, 401, "Usuário não encontrado");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            _logger.LogInformation("SignIn result: {Result}", result);

            if (!result.Succeeded)
            {
                return new Response<UserLogin>(null, 401, "Credenciais inválidas");
            }

            var token = _jwtTokenService.GenerateToken(user);
            var response = new UserLogin(request.Email, token);

            return new Response<UserLogin>(response, 200, "Login realizado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during the login process.");
            return new Response<UserLogin>(null, 500, "Erro interno no servidor");
        }
    }

    [HttpPost("createuser")]
    public async Task<Response<ApplicationUser?>> CreateUserAsync([FromBody] CreateApplicationUserRequest request)
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

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new Response<ApplicationUser?>(null, 400, "Não foi possível criar o usuário");
            }

            return new Response<ApplicationUser?>(user, 201, "Usuário criado com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the user.");
            return new Response<ApplicationUser?>(null, 500, "Erro interno no servidor");
        }
    }

    [HttpPut("updateuser")]
    public async Task<Response<ApplicationUser?>> UpdateUserAsync([FromBody] UpdateApplicationUserRequest request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

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

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return new Response<ApplicationUser?>(user, message: "Usuário atualizado com sucesso!");

            return new Response<ApplicationUser?>(null, 400, "Não foi possível atualizar o usuário");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the user.");
            return new Response<ApplicationUser?>(null, 500, "Erro interno no servidor");
        }
    }

    [HttpDelete("deleteuser")]
    public async Task<Response<ApplicationUser?>> DeleteUserAsync([FromBody] DeleteApplicationUserRequest request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
            {
                return new Response<ApplicationUser?>(null, 404, "Usuário não encontrado");
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return new Response<ApplicationUser?>(null, 400, "Não foi possível excluir o usuário");
            }

            return new Response<ApplicationUser?>(user, message: "Usuário excluído com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the user.");
            return new Response<ApplicationUser?>(null, 500, "Erro interno no servidor");
        }
    }

    [HttpGet("allusers")]
    public async Task<PagedResponse<List<ApplicationUser>>> GetAllUsersAsync(
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        try
        {
            var query = _context
                .Users
                .AsNoTracking()
                //.Where(x => x.Id == request.UserId)
                .OrderBy(u => u.Name)
                .ThenBy(u => u.Id);

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Cast<ApplicationUser>()
                .ToListAsync();

            var count = await query.CountAsync();
            return count == 0
                ? new PagedResponse<List<ApplicationUser>>(null, 404, "Usuário não encontrado")
                : new PagedResponse<List<ApplicationUser>>(users, count, pageNumber, pageSize);
        }
        catch
        {
            return new PagedResponse<List<ApplicationUser>>(null, 500, "Não foi possível consultar os usuários");
        }
    }

    [HttpPost("userbyid")]
    public async Task<Response<ApplicationUser?>> GetUserByIdAsync([FromBody] GetApplicationUserByIdRequest request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
            {
                return new Response<ApplicationUser?>(null, 404, "Usuário não encontrado");
            }

            return new Response<ApplicationUser?>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the user.");
            return new Response<ApplicationUser?>(null, 500, "Não foi possível recuperar o usuário");
        }
    }

    [HttpPost("userbycpf")]
    public async Task<Response<ApplicationUser?>> GetUserByCpfAsync([FromBody] GetApplicationUserByCpfRequest request)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Cpf == request.Cpf);

            if (user == null)
            {
                return new Response<ApplicationUser?>(null, 404, "Usuário não encontrado");
            }

            return new Response<ApplicationUser?>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the user.");
            return new Response<ApplicationUser?>(null, 500, "Não foi possível recuperar o usuário");
        }
    }

    [HttpPost("userbyname")]
    public async Task<PagedResponse<List<ApplicationUser>>> GetUserByNameAsync(
        [FromBody] GetApplicationUserByNameRequest request,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        try
        {
            var query = _context
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
            _logger.LogError(ex, "An error occurred while getting the user.");
            return new PagedResponse<List<ApplicationUser>>(null, 500, "Não foi possível recuperar o usuário");
        }
    }

    [HttpPost("userbyfullname")]
    public async Task<PagedResponse<List<ApplicationUser>>> GetUserByFullNameAsync(
        [FromBody] GetApplicationUserByFullNameRequest request,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        try
        {
            var query = _context
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
            _logger.LogError(ex, "An error occurred while getting the user.");
            return new PagedResponse<List<ApplicationUser>>(null, 500, "Não foi possível recuperar o usuário");
        }
    }
}