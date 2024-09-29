namespace RDS.Api.Handlers;

public class ApplicationUserConfigurationHandler(
    RoleManager<ApplicationRole> roleManager,
    UserService userService,
    AppDbContext appDbContext,
    UserManager<User> userManager) : IApplicationUserConfigurationHandler
{
    public async Task<Response<ApplicationRole?>> CreateRoleAsync(CreateApplicationRoleRequest request)
    {
        var roleName = request.Name.Capitalize();
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name) || await roleManager.RoleExistsAsync(roleName))
            {
                return new Response<ApplicationRole?>(null, 401, "Nome da role inválido!");
            }

            //var result = await roleManager.CreateAsync(new IdentityRole<long>(roleName));
            //var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
            var role = new ApplicationRole { Name = roleName };
            var result = await roleManager.CreateAsync(role);
            if (!result.Succeeded)
                return new Response<ApplicationRole?>(
                    null, 401, "Ocorreu um erro ao criar a role!");

            var response = new ApplicationRole { Name = roleName };
            return new Response<ApplicationRole?>(response, 201, "Role criada com sucesso!");
        }
        catch
        {
            return new Response<ApplicationRole?>(null, 500, "Erro interno no servidor");
        }
    }

    public async Task<Response<ApplicationRole?>> DeleteRoleAsync(DeleteApplicationRoleRequest request)
    {
        var roleName = request.Name.Capitalize();
        try
        {
            if (roleName is "Admin" or "Owner" or "User")
                return new Response<ApplicationRole?>(
                    null, 401, "Não é possível excluir a Role Admin ou User.");

            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return new Response<ApplicationRole?>(null, 404, "Role não encontrada.");
            }

            var result = await roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                return new Response<ApplicationRole?>(null, 401, "Erro ao excluir a Role.");

            var response = new ApplicationRole
            {
                Id = role.Id,
                Name = role.Name
            };
            return new Response<ApplicationRole?>(response, 200, "Role excluída com sucesso!");
        }
        catch
        {
            return new Response<ApplicationRole?>(null, 500, "500 x - Erro interno no servidor");
        }
    }

    public async Task<PagedResponse<List<ApplicationRole?>>> ListRoleAsync()
    {
        try
        {
            var roles = await roleManager.Roles.ToListAsync();

            var response = roles.Select(role => new ApplicationRole
                {
                    Id = role.Id,
                    Name = role.Name
                })
                .OrderBy(role => role.Name != "Admin")
                .ThenBy(role => role.Name != "Owner")
                .ThenBy(role => role.Name != "User")
                .ThenBy(role => role.Name)
                .ToList();

            return new PagedResponse<List<ApplicationRole?>>(
                response!, 200, "Roles listadas com sucesso!");
        }
        catch
        {
            return new PagedResponse<List<ApplicationRole?>>(
                null, 500, "Erro interno no servidor");
        }
    }

    public async Task<Response<ApplicationUserRole?>> CreateUserRoleAsync(CreateApplicationUserRoleRequest request)
    {
        await using var transaction = await appDbContext.Database.BeginTransactionAsync();
        var roleId = request.RoleId.ToString();
        try
        {
            var user = await userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return new Response<ApplicationUserRole?>(null, 404, "Usuário não encontrado.");
            }

            var roleToAdd = await roleManager.FindByIdAsync(roleId);

            if (roleToAdd == null)
            {
                return new Response<ApplicationUserRole?>(null, 404, "Role não encontrada..");
            }

            var userRole = new ApplicationUserRole
            {
                UserId = user.Id,
                RoleId = request.RoleId,
                CompanyId = request.CompanyId,
                RoleName = roleToAdd.Name!.Capitalize()
            };
            await appDbContext.IdentityUsersRoles.AddAsync(userRole);
            var saveResult = await appDbContext.SaveChangesAsync();

            if (saveResult < 1)
                return new Response<ApplicationUserRole?>(
                    null, 401, "Erro ao atribuir a Role ao usuário.");

            var response = new ApplicationUserRole
            {
                UserId = request.CompanyId,
                RoleId = request.RoleId,
            };

            await transaction.CommitAsync();

            return new Response<ApplicationUserRole?>(
                response, 201, "Role atribuída ao usuário com sucesso!");
        }
        catch
        {
            await transaction.RollbackAsync();
            return new Response<ApplicationUserRole?>(null, 500, "Erro interno no servidor");
        }
    }

    public async Task<Response<ApplicationUserRole?>> DeleteUserRoleAsync(DeleteApplicationUserRoleRequest request)
    {
        var roleName = request.RoleName.Capitalize();
        try
        {
            if (roleName is "Admin" or "Owner" or "User")
                return new Response<ApplicationUserRole?>(
                    null, 401, $"Não é possível excluir a Role {roleName}.");

            var user = await userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return new Response<ApplicationUserRole?>(null, 404, "Usuário não encontrado.");
            }

            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                return new Response<ApplicationUserRole?>(null, 404, "Role não encontrada..");
            }

            var result = await userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
                return new Response<ApplicationUserRole?>(
                    null, 401, "Erro ao excluir a Role ao usuário.");

            var response = new ApplicationUserRole
            {
                UserId = request.CompanyId,
                RoleId = request.RoleId,
            };

            return new Response<ApplicationUserRole?>(
                response, 200, "Role do usuário excluída com sucesso!");
        }
        catch
        {
            return new Response<ApplicationUserRole?>(null, 500, "Erro interno no servidor");
        }
    }

    public async Task<PagedResponse<List<ApplicationUserRole?>>> ListUserRoleAsync(
        GetAllApplicationUserRoleRequest request)
    {
        var userId = request.UserId;
        var companyId = request.CompanyId;
        try
        {
            // var company = await appDbContext.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
            // if (company == null)
            //     return  new PagedResponse<List<ApplicationUserRole?>>(
            //         null, 500, "NFC - Erro interno no servidor");
            //
            //var companyName = company?.Name ?? string.Empty;


            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new PagedResponse<List<ApplicationUserRole?>>(
                    [], 404, "Usuário não encontrado.");
            }

            var roles = (await userManager.GetRolesAsync(user)).Distinct().ToList();
            var response = roles.Select(roleName => new ApplicationUserRole
                {
                    RoleName = roleName,
                    RoleId = roleManager.Roles.FirstOrDefault(
                        x => x.Name == roleName)?.Id ?? 0,
                    CompanyId = companyId,
                    UserId = userId,
                })
                .OrderBy(role => role.RoleName != "Admin")
                .ThenBy(role => role.RoleName != "Owner")
                .ThenBy(role => role.RoleName != "User")
                .ToList();

            return new PagedResponse<List<ApplicationUserRole?>>(response!, 200,
                "Roles do usuário listadas com sucesso!");
        }
        catch
        {
            return new PagedResponse<List<ApplicationUserRole?>>(
                null, 500, "Erro interno no servidor");
        }
    }

    public async Task<PagedResponse<List<ApplicationUserRole?>>> ListRolesForAddToUserAsyncOld(
        GetAllApplicationUserRoleRequest request)
    {
        if (!request.RoleAuthorization
            || !userService.VerifyIfIsInRole(request.Token, request.Roles).Result.existRole)
        {
            return new PagedResponse<List<ApplicationUserRole?>>(
                null, 403, "Operação não permitida");
        }

        //var typeRole = userService.VerifyIfIsInRole(request.Token, request.Roles).Result.typeRole;
        var userId = request.UserId;
        //var companyId = request.CompanyId;
        try
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new PagedResponse<List<ApplicationUserRole?>>(
                    null, 404, "Usuário não encontrado.");
            }

            var allRoles = roleManager.Roles.ToList();

            var userRoles = await userManager.GetRolesAsync(user);

            var rolesNotForUser = allRoles
                .Where(role => !userRoles.Contains(role.Name!))
                .Select(role => new ApplicationUserRole
                {
                    RoleName = role.Name!,
                    RoleId = role.Id,
                    UserId = userId,
                })
                .OrderBy(role => role.RoleName != "Admin")
                .ThenBy(role => role.RoleName != "Owner")
                .ThenBy(role => role.RoleName != "User")
                .ThenBy(role => role.RoleName)
                .ToList();

            return new PagedResponse<List<ApplicationUserRole?>>(rolesNotForUser!, 200,
                "Roles disponíveis para adicionar ao usuário listadas com sucesso!");
        }
        catch
        {
            return new PagedResponse<List<ApplicationUserRole?>>(
                null, 500, "Erro interno no servidor");
        }
    }

    public async Task<PagedResponse<List<ApplicationUserRole?>>> ListRolesForAddToUserAsync(
        GetAllApplicationUserRoleRequest request)
    {
        var resultInRole = userService.VerifyIfIsInRole(request.Token, request.Roles);
        if (!request.RoleAuthorization
            || !resultInRole.Result.existRole)
        {
            return new PagedResponse<List<ApplicationUserRole?>>(
                null, 403, "Operação não permitida");
        }

        var typeRole = resultInRole.Result.typeRole;
        var userId = request.UserId;
        var companyId = request.CompanyId;

        try
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new PagedResponse<List<ApplicationUserRole?>>(
                    null, 404, "Usuário não encontrado.");
            }
            
            // Se o tipo de role não for "Admin", buscar os usuários correspondentes ao companyId
            if (typeRole != "Admin")
            {
                var companyUsers = await appDbContext.CompanyUsers
                    .Where(cu => cu.CompanyId == companyId)
                    .Select(cu => cu.UserId)
                    .ToListAsync();

                // Verifica se o usuário está associado a essa empresa
                if (!companyUsers.Contains(userId))
                {
                    return new PagedResponse<List<ApplicationUserRole?>>(
                        null, 403, "Usuário não pertence à empresa especificada.");
                }
            }

            var allRoles = roleManager.Roles.ToList();
            var userRoles = (await userManager.GetRolesAsync(user)).Distinct().ToList();
            
            var rolesNotForUser = allRoles
                .Where(role => !userRoles.Contains(role.Name!))
                .Select(role => new ApplicationUserRole
                {
                    RoleName = role.Name!,
                    RoleId = role.Id,
                    UserId = userId,
                })
                .OrderBy(role => role.RoleName != "Admin")
                .ThenBy(role => role.RoleName != "Owner")
                .ThenBy(role => role.RoleName != "User")
                .ThenBy(role => role.RoleName)
                .ToList();

            if (typeRole != "Admin")
            {
                rolesNotForUser.RemoveAll(x => x.RoleName == "Admin" || x.RoleName == "Owner");
            }

            return new PagedResponse<List<ApplicationUserRole?>>(rolesNotForUser!, 200,
                "Roles disponíveis para adicionar ao usuário listadas com sucesso!");
        }
        catch
        {
            return new PagedResponse<List<ApplicationUserRole?>>(
                null, 500, "Erro interno no servidor");
        }
    }
}