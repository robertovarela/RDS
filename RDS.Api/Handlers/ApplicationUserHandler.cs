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

    public async Task<Response<ApplicationUser?>> UpdateAsync(UpdateApplicationUserRequest request)
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

    public async Task<Response<ApplicationUser?>> DeleteAsync(DeleteApplicationUserRequest request)
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

    public async Task<PagedResponse<List<AllUsersViewModel>>> GetAllAsync(GetAllApplicationUserRequest request)
    {
        try
        {
            string notFoundMessage;
            long.TryParse(request.Filter, out long filterId);
            string filter = request.Filter ?? string.Empty;
            IQueryable<AllUsersViewModel> query;

            if (filterId != 0)
            {
                if (filter.Length == 11)
                {
                    query = context.Users.AsNoTracking()
                        .Where(u => u.Cpf == request.Filter)
                        .Select(u => new AllUsersViewModel { Id = u.Id, Name = u.Name, Email = u.Email! });

                    notFoundMessage = "CPF não localizado";
                }
                else
                {
                    query = context.Users.AsNoTracking()
                        .Where(u => u.Id == filterId)
                        .Select(u => new AllUsersViewModel { Id = u.Id, Name = u.Name, Email = u.Email! });

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
                    .Select(u => new AllUsersViewModel { Id = u.Id, Name = u.Name, Email = u.Email! });

                notFoundMessage = filter.Contains('@') ? "Email não localizado" : "Usuário não encontrado";
            }

            var users = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .OrderBy(users => users.Name)
                .ToListAsync();

            var count = await query.CountAsync();
            return count == 0
                ? new PagedResponse<List<AllUsersViewModel>>(null, 404, notFoundMessage)
                : new PagedResponse<List<AllUsersViewModel>>(users, count, request.PageNumber, request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<AllUsersViewModel>>(null, 500, "Não foi possível consultar os usuários");
        }
    }

    public async Task<PagedResponse<List<AllUsersViewModel>>> GetAllByCompanyIdAsync(GetAllApplicationUserRequest request)
    {
         try
        {
            string notFoundMessage;
            long.TryParse(request.Filter, out long filterId);
            IQueryable<AllUsersViewModel> query;

            if (filterId != 0)
            {
                if (request.Filter.Length == 11)
                {
                    query = from u in context.Users.AsNoTracking()
                        join cu in context.CompanyUsers.AsNoTracking()
                            on u.Id equals cu.UserId
                        where cu.CompanyId == request.CompanyId && u.Cpf == request.Filter
                        select new AllUsersViewModel { Id = u.Id, Name = u.Name, Email = u.Email! };

                    notFoundMessage = "CPF não localizado";
                }
                else
                {
                    query = from u in context.Users.AsNoTracking()
                        join cu in context.CompanyUsers.AsNoTracking()
                            on u.Id equals cu.UserId
                        where cu.CompanyId == request.CompanyId && u.Id == filterId
                        select new AllUsersViewModel { Id = u.Id, Name = u.Name, Email = u.Email! };

                    notFoundMessage = "Código não localizado";
                }
            }
            else
            {
                query = from u in context.Users.AsNoTracking()
                    join cu in context.CompanyUsers.AsNoTracking()
                        on u.Id equals cu.UserId
                    where cu.CompanyId == request.CompanyId &&
                          ((string.IsNullOrEmpty(request.Filter) || u.Name.StartsWith(request.Filter)) ||
                           (string.IsNullOrEmpty(request.Filter) || u.Email == request.Filter))
                    orderby u.Id
                    select new AllUsersViewModel { Id = u.Id, Name = u.Name!, Email = u.Email! };

                notFoundMessage = request.Filter.Contains("@") ? "Email não localizado" : "Usuário não encontrado";
            }

            var count = await query.CountAsync();

            if (count == 0)
                return new PagedResponse<List<AllUsersViewModel>>(null, 404, notFoundMessage);

            var users = await query
                .OrderBy(users => users.Name)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PagedResponse<List<AllUsersViewModel>>(users, count, request.PageNumber, request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<AllUsersViewModel>>(null, 500, "Não foi possível consultar os usuários");
        }
    }

    public async Task<Response<ApplicationUser?>> GetByIdAsync(GetApplicationUserByIdRequest request)
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

    public async Task<Response<ApplicationUser?>> GetByCpfAsync(GetApplicationUserByCpfRequest request)
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

    public async Task<PagedResponse<List<ApplicationUser>>> GetByNameAsync(GetApplicationUserByNameRequest request)
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
                : new PagedResponse<List<ApplicationUser>>(users, count, request.PageNumber, request.PageSize);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting the user.");
            return new PagedResponse<List<ApplicationUser>>(null, 500, "Não foi possível recuperar o usuário");
        }
    }

    public async Task<PagedResponse<List<ApplicationUser>>> GetByFullNameAsync(GetApplicationUserByFullNameRequest request)
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
                : new PagedResponse<List<ApplicationUser>>(users, count, request.PageNumber, request.PageSize);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting the user.");
            return new PagedResponse<List<ApplicationUser>>(null, 500, "Não foi possível recuperar o usuário");
        }
    }
}