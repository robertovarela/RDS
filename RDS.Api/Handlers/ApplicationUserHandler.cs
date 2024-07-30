namespace RDS.Api.Handlers
{
    public class ApplicationUserHandler(UserManager<User> userManager, AppDbContext context) : IApplicationUserHandler
    {
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

                return new Response<ApplicationUser?>(user, 201, "Usuário criado com sucesso!");
            }
            catch
            {
                return new Response<ApplicationUser?>(null, 500, "Não foi possível criar o usuário");
            }
        }

        public async Task<Response<ApplicationUser?>> DeleteAsync(DeleteApplicationUserRequest request)
        {
            try
            {
                var user = await userManager.FindByIdAsync(request.UserId.ToString());

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
            catch
            {
                return new Response<ApplicationUser?>(null, 500, "Não foi possível excluir o usuário");
            }
        }

        public async Task<PagedResponse<List<ApplicationUser>>> GetAllAsync(GetAllApplicationUserRequest request)
        {
            try
            {
                var query = context
                    .Users
                    .AsNoTracking()
                    //.Where(x => x.Id == request.UserId)
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

        public async Task<Response<ApplicationUser?>> GetByCpfAsync(GetApplicationUserByCpfRequest request)
        {
            try
            {
                var user = await context.Users
                    .FirstOrDefaultAsync(u => u.Cpf == request.Cpf);

                return user == null
                    ? new Response<ApplicationUser?>(null, 404, "Usuário não encontrado")
                    : new Response<ApplicationUser?>(user);
            }
            catch
            {
                return new Response<ApplicationUser?>(null, 500, "Não foi possível recuperar o usuário");
            }
        }

        public async Task<Response<ApplicationUser?>> GetByIdAsync(GetApplicationUserByIdRequest request)
        {
            try
            {
                var user = await userManager.FindByIdAsync(request.UserId.ToString());

                return user == null
                    ? new Response<ApplicationUser?>(null, 404, "Usuário não encontrado")
                    : new Response<ApplicationUser?>(user);
            }
            catch
            {
                return new Response<ApplicationUser?>(null, 500, "Não foi possível recuperar o usuário");
            }
        }

        public async Task<Response<ApplicationUser?>> GetActiveAsync(GetApplicationUserActiveRequest request)
        {
            try
            {
                var user = await userManager.FindByIdAsync(request.UserId.ToString());

                return user == null
                    ? new Response<ApplicationUser?>(null, 404, "Usuário não encontrado")
                    : new Response<ApplicationUser?>(user);
            }
            catch
            {
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
            catch
            {
                return new PagedResponse<List<ApplicationUser>>(null, 500, "Não foi possível recuperar os usuários");
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
            catch
            {
                return new PagedResponse<List<ApplicationUser>>(null, 500, "Não foi possível recuperar o usuário");
            }
        }

        public async Task<Response<ApplicationUser?>> UpdateAsync(UpdateApplicationUserRequest request)
        {
            try
            {
                var user = await userManager.FindByIdAsync(request.UserId.ToString());

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

                if (!result.Succeeded)
                {
                    return new Response<ApplicationUser?>(null, 400, "Não foi possível atualizar o usuário");
                }

                return new Response<ApplicationUser?>(user, message: "Usuário atualizado com sucesso!");
            }
            catch
            {
                return new Response<ApplicationUser?>(null, 500, "Não foi possível atualizar o usuário");
            }
        }
    }
}