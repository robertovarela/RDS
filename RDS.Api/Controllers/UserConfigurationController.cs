namespace RDS.Api.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("v1/userconfiguration")]
public class UserConfigurationController(
    RoleManager<IdentityRole<long>> roleManager,
    UserManager<User> userManager) : Controller
{
    [HttpPost("create-role")]
    public async Task<Response<ApplicationRole>> CreateRole([FromBody] CreateApplicationRoleRequest request)
    {
        var roleName = request.Name.Capitalize();
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name) || await roleManager.RoleExistsAsync(roleName))
            {
                return new Response<ApplicationRole>(null, 401, "Nome da role inválido!");
            }

            var result = await roleManager.CreateAsync(new IdentityRole<long>(roleName));

            if (!result.Succeeded) return new Response<ApplicationRole>(null, 401, "Ocorreu um erro ao criar a role!");
            var response = new ApplicationRole { Name = roleName };
            return new Response<ApplicationRole>(response, 201, "Role criada com sucesso!");
        }
        catch
        {
            return new Response<ApplicationRole>(null, 500, "Erro interno no servidor");
        }
    }

    [HttpDelete("delete-role")]
    public async Task<Response<ApplicationRole>> DeleteRole([FromBody] DeleteApplicationRoleRequest request)
    {
        var roleName = request.Name.Capitalize();
        try
        {
            if (roleName is "Admin" or "Owner" or "User")
                return new Response<ApplicationRole>(null, 401, "Não é possível excluir a Role Admin ou User.");

            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return new Response<ApplicationRole>(null, 404, "Role não encontrada.");
            }

            var result = await roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                return new Response<ApplicationRole>(null, 401, "Erro ao excluir a Role.");

            var response = new ApplicationRole
            {
                Id = role.Id,
                Name = role.Name
            };
            return new Response<ApplicationRole>(response, 200, "Role excluída com sucesso!");
        }
        catch
        {
            return new Response<ApplicationRole>(null, 500, "500 x - Erro interno no servidor");
        }
    }

    [HttpPost("list-roles")]
    public async Task<Response<List<ApplicationRole>>> ListRoles()
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
            return new Response<List<ApplicationRole>>(response, 200, "Roles listadas com sucesso!");
        }
        catch
        {
            return new Response<List<ApplicationRole>>(null, 500, "Erro interno no servidor");
        }
    }

    [HttpPost("create-role-to-user")]
    public async Task<Response<ApplicationUserRole>> CreateRoleToUser(CreateApplicationUserRoleRequest request)
    {
        var roleName = request.RoleName.Capitalize();
        try
        {
            var user = await userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return new Response<ApplicationUserRole>(null, 404, "Usuário não encontrado.");
            }

            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                return new Response<ApplicationUserRole>(null, 404, "Role não encontrada..");
            }

            var result = await userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
                return new Response<ApplicationUserRole>(null, 401, "Erro ao atribuir a Role ao usuário.");

            var response = new ApplicationUserRole
            {
                UserId = request.UserId,
                RoleId = request.RoleId,
            };

            return new Response<ApplicationUserRole>(response, 201, "Role atribuída ao usuário com sucesso!");
        }
        catch
        {
            return new Response<ApplicationUserRole>(null, 500, "Erro interno no servidor");
        }
    }

    [HttpDelete("delete-role-to-user")]
    public async Task<Response<ApplicationUserRole>> DeleteRoleToUser(DeleteApplicationUserRoleRequest request)
    {
        var roleName = request.RoleName.Capitalize();
        try
        {
            if (roleName is "Admin" or "Owner" or "User")
                return new Response<ApplicationUserRole>(null, 401, $"Não é possível excluir a Role {roleName}.");

            var user = await userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return new Response<ApplicationUserRole>(null, 404, "Usuário não encontrado.");
            }

            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                return new Response<ApplicationUserRole>(null, 404, "Role não encontrada..");
            }

            var result = await userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
                return new Response<ApplicationUserRole>(null, 401, "Erro ao excluir a Role ao usuário.");

            var response = new ApplicationUserRole
            {
                UserId = request.UserId,
                RoleId = request.RoleId,
            };

            return new Response<ApplicationUserRole>(response, 200, "Role do usuário excluída com sucesso!");
        }
        catch
        {
            return new Response<ApplicationUserRole>(null, 500, "Erro interno no servidor");
        }
    }

    [HttpPost("list-roles-for-user")]
    public async Task<Response<List<ApplicationUserRole>>> ListRolesForUser(GetAllApplicationUserRoleRequest request)
    {
        var userId = request.UserId;
        try
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Response<List<ApplicationUserRole>>(null, 404, "Usuário não encontrado.");
            }

            var roles = await userManager.GetRolesAsync(user);
            var response = roles.Select(roleName => new ApplicationUserRole
            {
                RoleName = roleName,
                RoleId = roleManager.Roles.FirstOrDefault(x => x.Name == roleName)?.Id ?? 0,
                UserId = userId,
            }).ToList();

            return new Response<List<ApplicationUserRole>>(response, 200, "Roles do usuário listadas com sucesso!");
        }
        catch
        {
            return new Response<List<ApplicationUserRole>>(null, 500, "Erro interno no servidor");
        }
    }

    [HttpPost("list-roles-not-for-user")]
    public async Task<Response<List<ApplicationUserRole>>> ListRolesNotForUser(GetAllApplicationUserRoleRequest request)
    {
        var userId = request.UserId;
        try
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Response<List<ApplicationUserRole>>(null, 404, "Usuário não encontrado.");
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

            return new Response<List<ApplicationUserRole>>(rolesNotForUser, 200,
                "Roles disponíveis para adicionar ao usuário listadas com sucesso!");
        }
        catch
        {
            return new Response<List<ApplicationUserRole>>(null, 500, "Erro interno no servidor");
        }
    }
}