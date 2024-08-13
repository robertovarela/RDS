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
    
    [HttpPost("delete-role")]
    public async Task<Response<ApplicationRole>> DeleteRole([FromBody] DeleteApplicationRoleRequest request)
    {
        var roleName = request.Name.Capitalize();
        try
        {
            if(roleName is "Admin" or "User")
                return new Response<ApplicationRole>(null, 401, "Não é possível excluir a Role Admin ou User.");

            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return new Response<ApplicationRole>(null, 404, "Role não encontrada.");
            }

            var result = await roleManager.DeleteAsync(role);
            if(!result.Succeeded)
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
            }).ToList();

            return new Response<List<ApplicationRole>>(response, 200, "Roles listadas com sucesso!");
        }
        catch
        {
            return new Response<List<ApplicationRole>>(null, 500, "Erro interno no servidor");
        }
    }

    [HttpPost("assign-role-to-user")]
    public async Task<Response<ApplicationUserRole>> AssignRoleToUser(CreateApplicationUserRoleRequest request)
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
    
    [HttpPost("delete-role-to-user")]
    public async Task<Response<ApplicationUserRole>> DeleteRoleToUser(DeleteApplicationUserRoleRequest request)
    {
        var roleName = request.RoleName.Capitalize();
        try
        {
            if(roleName is "Admin" or "User")
                return new Response<ApplicationUserRole>(null, 401, "Não é possível excluir a Role Admin ou User.");
            
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

            return new Response<ApplicationUserRole>(response, 201, "Role do usuário excluída com sucesso!");
        }
        catch
        {
            return new Response<ApplicationUserRole>(null, 500, "Erro interno no servidor");
        }
    }
    
    [HttpPost("list-roles-for-user")]
    public async Task<Response<List<ApplicationRole>>> ListRolesForUser(GetApplicationUserByIdRequest request)
    {
        var userId = request.UserId;
        try
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Response<List<ApplicationRole>>(null, 404, "Usuário não encontrado.");
            }

            var roles = await userManager.GetRolesAsync(user);
            var response = roles.Select(roleName => new ApplicationRole
            {
                Name = roleName
            }).ToList();

            return new Response<List<ApplicationRole>>(response, 200, "Roles do usuário listadas com sucesso!");
        }
        catch
        {
            return new Response<List<ApplicationRole>>(null, 500, "Erro interno no servidor");
        }
    }
}