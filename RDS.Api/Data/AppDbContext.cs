using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RDS.Api.Models;
using RDS.Core.Models;
using RDS.Core.Models.ApplicationUser;
using RDS.Core.Models.Reports;
using System.Reflection;
using RDS.Core.Models.Company;

namespace RDS.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User,
        IdentityRole<long>,
        long,
        IdentityUserClaim<long>,
        IdentityUserRole<long>,
        IdentityUserLogin<long>,
        IdentityRoleClaim<long>,
        IdentityUserToken<long>>(options)
{
    public DbSet<ApplicationUserAddress> Addresses { get; set; } = null!;
    public DbSet<ApplicationUserTelephone> Telephones { get; set; } = null!;
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<CompanyUser> CompanyUsers { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;


    //Tables generated from views
    public DbSet<IncomesAndExpenses> IncomesAndExpenses { get; set; } = null!;
    public DbSet<IncomesByCategory> IncomesByCategories { get; set; } = null!;
    public DbSet<ExpensesByCategory> ExpensesByCategories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public void EnsureDatabaseCreatedWithViews()
    {
        Database.EnsureCreated();

        var assembly = Assembly.GetExecutingAssembly();
        var resourceNames = new[]
            {
                "RDS.Api.Data.SqlScripts.vwGetIncomesByCategory.sql",
                "RDS.Api.Data.SqlScripts.vwGetExpensesByCategory.sql",
                "RDS.Api.Data.SqlScripts.vwGetIncomesAndExpenses.sql"
            };

        foreach (var resourceName in resourceNames)
        {
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                using var reader = new StreamReader(stream);
                var sql = reader.ReadToEnd();

                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = sql;
                Database.OpenConnection();
                command.ExecuteNonQuery();
                Database.CloseConnection();
            }
        }
    }
}