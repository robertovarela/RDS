namespace RDS.Api.Handlers;

public class ApplicationUserHandler(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    JwtTokenService jwtTokenService,
    ILogger<ApplicationUserHandler> logger,
    AppDbContext context)
    : IApplicationUserHandler
{

    public async Task<Response<UserLogin>> LoginAsync(LoginRequest? request)
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

    public async Task<Response<UserRefreshToken>> RefreshTokenAsync(RefreshTokenRequest? request)
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

    public async Task<Response<ApplicationUser?>> CreateAsync(CreateApplicationUserRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<ApplicationUser?>> UpdateAsync(UpdateApplicationUserRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<ApplicationUser?>> DeleteAsync(DeleteApplicationUserRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResponse<List<ApplicationUser>>> GetAllAsync(GetAllApplicationUserRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResponse<List<ApplicationUser>>> GetAllByCompanyIdAsync(GetAllApplicationUserRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<ApplicationUser?>> GetByIdAsync(GetApplicationUserByIdRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<ApplicationUser?>> GetActiveAsync(GetApplicationUserActiveRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<ApplicationUser?>> GetByCpfAsync(GetApplicationUserByCpfRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResponse<List<ApplicationUser>>> GetByNameAsync(GetApplicationUserByNameRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResponse<List<ApplicationUser>>> GetByFullNameAsync(GetApplicationUserByFullNameRequest request)
    {
        throw new NotImplementedException();
    }
}