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
    public static void ConfigureDevEnvironment(this WebApplication app)
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
}