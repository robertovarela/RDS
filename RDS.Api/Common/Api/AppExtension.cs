namespace RDS.Api.Common.Api;

public static class AppExtension
{
    private static void LoadConfiguration(this WebApplication app)
    {
        ApiConfiguration.JwtKey = app.Configuration.GetValue<string>("Jwt:Key") 
                               ?? throw new InvalidOperationException("JwtKey is not configured.");
        ApiConfiguration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName") 
                                   ?? throw new InvalidOperationException("ApiKeyName is not configured.");
        ApiConfiguration.ApiKey = app.Configuration.GetValue<string>("ApiKey") 
                               ?? throw new InvalidOperationException("ApiKey is not configured.");

        var smtp = new ApiConfiguration.SmtpConfiguration();
        app.Configuration.GetSection("SmtpConfiguration").Bind(smtp);
        ApiConfiguration.Smtp = smtp;
    }
    
    public static void AppConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.ConfigureDevEnvironment();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
            app.UseHttpsRedirection();
        }
        
        app.LoadConfiguration();
        app.UseCors(ApiConfiguration.CorsPolicyName);
        app.UseSecurity();
        app.MapControllers();
        app.UseStaticFiles();
        app.UseResponseCompression();
    }

    private static void ConfigureDevEnvironment(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.MapSwagger().RequireAuthorization();
    }

    private static void UseSecurity(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
    
    public static async Task SeedDatabase(this WebApplication app)
    {
        // ReSharper disable once ConvertToUsingDeclaration
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<AppDbContext>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
                
                var seedData = new SeedData(context, userManager, roleManager);
                await seedData.InitializeAsync();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Ocorreu um erro ao semear o banco de dados.");
            }
        }

    }
}