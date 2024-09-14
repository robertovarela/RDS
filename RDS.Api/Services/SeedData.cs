using Microsoft.Data.SqlClient;

namespace RDS.Api.Services;

public class SeedData(
    AppDbContext context,
    UserManager<User> userManager,
    RoleManager<ApplicationRole> roleManager)
{
    public async Task InitializeAsync()
    {
        await context.Database.EnsureCreatedAsync();

        await SeedRolesAsync();
        await SeedUsersAsync();
        await SeedCompaniesAsync();
    }

    private async Task ResetIdentityIdsAsync(string table)
    {
        var sql = "DBCC CHECKIDENT (@TableName, RESEED, 0)";
        var parameter = new SqlParameter("@TableName", table);

        await context.Database.ExecuteSqlRawAsync(sql, parameter);
    }

    private async Task SeedRolesAsync()
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            if (!roleManager.Roles.Any())
            {
                var table = "IdentityRole";
                await ResetIdentityIdsAsync(table);
                var roles = new[] { "Admin", "Owner", "User" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new ApplicationRole { Name = role });
                    }
                }

                await transaction.CommitAsync();
            }
        }
        catch
        {
            await transaction.RollbackAsync();
        }
    }

    private async Task SeedUsersAsync()
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            if (!context.IdentityUsers.Any())
            {
                var table = "IdentityUser";
                await ResetIdentityIdsAsync(table);
                var adminUser = new User
                {
                    UserName = "rdsadmin@mysoftwares.com.br",
                    Email = "rdsadmin@mysoftwares.com.br",
                    Name = "RDS - Admin",
                    CreateAt = DateTime.Now,
                    EmailConfirmed = true,
                };

                var result = await userManager.CreateAsync(adminUser, "w3297568@#W3");

                if (result.Succeeded)
                {
                    var userRole = new ApplicationUserRole
                        { UserId = 1, RoleId = 1, CompanyId = 1, RoleName = "Admin" };
                    await context.IdentityUsersRoles.AddAsync(userRole);
                    await context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                else
                {
                    throw new Exception("Falha ao criar o usuário admin");
                }
            }
        }
        catch
        {
            await transaction.RollbackAsync();
        }
    }

    private async Task SeedCompaniesAsync()
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            if (!context.Companies.Any())
            {
                var table = "Company";
                await ResetIdentityIdsAsync(table);
                var adminUser = await userManager.FindByEmailAsync("rdsadmin@mysoftwares.com.br");
                if (adminUser != null)
                {
                    var company = new Company
                    {
                        Name = "RDS* - Empresa Padrão",
                        Description = "Empresa para testes administrativos",
                        OwnerId = adminUser.Id
                    };

                    context.Companies.Add(company);
                    await context.SaveChangesAsync();
                    
                    var companyUser = new CompanyUser
                    {
                        CompanyId = company.Id,
                        UserId = adminUser.Id,
                        IsAdmin = true
                    };
                    await context.CompanyUsers.AddAsync(companyUser);
                    await context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                else
                {
                    throw new Exception("Falha ao encontrar o usuário Admin");
                }
            }
        }
        catch
        {
            await transaction.RollbackAsync();
        }
    }

    // private static async Task SeedCompanyUserAsync(AppDbContext context)
    // {
    //     await using var transaction = await context.Database.BeginTransactionAsync();
    //     try
    //     {
    //         var firstCompany = context.Companies.FirstOrDefault();
    //         if (firstCompany != null)
    //         {
    //             var companyUser = new CompanyUser
    //             {
    //                 CompanyId = firstCompany.Id,
    //                 UserId = firstCompany.OwnerId,
    //                 IsAdmin = true
    //             };
    //
    //             await context.CompanyUsers.AddAsync(companyUser);
    //             await context.SaveChangesAsync();
    //
    //             await transaction.CommitAsync();
    //         }
    //         else
    //         {
    //             throw new Exception("Empresa padrão não localizada.");
    //         }
    //     }
    //     catch
    //     {
    //         await transaction.RollbackAsync();
    //     }
    // }
}