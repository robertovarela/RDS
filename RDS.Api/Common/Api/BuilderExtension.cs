namespace RDS.Api.Common.Api;

public static class BuilderExtension
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.ConnectionString = builder
            .Configuration
            .GetConnectionString("DefaultConnection") ?? string.Empty;
        Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
        Configuration.FrontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? string.Empty;
        ApiConfiguration.StripeApiKey = builder.Configuration.GetValue<string>("StripeApiKey") ?? string.Empty;

        StripeConfiguration.ApiKey = ApiConfiguration.StripeApiKey;
    }

    public static void AddKerstrel(this WebApplicationBuilder builder)
    {
        builder.WebHost.ConfigureKestrel(options =>
        {
            int port = Convert.ToInt32(Configuration.BackendUrl.Substring(Configuration.BackendUrl.Length - 4));

            if (builder.Environment.IsProduction())
            {
                options.ListenAnyIP(port, listenOptions =>
                {
                    listenOptions.UseHttps(); // Certifique-se de que o certificado SSL est� configurado corretamente
                });
            }
        });
    }

    public static void AddDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x => { x.CustomSchemaIds(n => n.FullName); });
    }

    public static void AddSecurity(this WebApplicationBuilder builder)
    {
        var key = Encoding.ASCII.GetBytes(ApiConfiguration.JwtKey);

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

        builder.Services.AddAuthorization();
    }

    public static void AddDataContexts(this WebApplicationBuilder builder)
    {
        builder
            .Services
            .AddDbContext<AppDbContext>(x => { x.UseSqlServer(Configuration.ConnectionString); });

        builder.Services
            .AddIdentityCore<User>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+" +
                    "áéíóúâêîôûãõàèìòùäëïöüñçÁÉÍÓÚÂÊÎÔÛÃÕÀÈÌÒÙÄËÏÖÜÑÇ ";
            })
            .AddRoles<IdentityRole<long>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager()
            .AddUserManager<UserManager<User>>()
            .AddDefaultTokenProviders()
            .AddApiEndpoints();

        using (var scope = builder.Services.BuildServiceProvider().CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.EnsureDatabaseCreatedWithViews();
        }
    }

    public static void AddCrossOrigin(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(
            options => options.AddPolicy(
                ApiConfiguration.CorsPolicyName,
                policy => policy
                    .WithOrigins(new[]
                    {
                        Configuration.BackendUrl,
                        Configuration.FrontendUrl,
                        "https://*.mysoftwares.com.br"
                    })
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithMethods("PUT", "DELETE", "GET", "POST", "OPTIONS")
                    .AllowAnyHeader()
                //.AllowCredentials()
            ));
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();

        builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();

        builder.Services.AddTransient<IReportHandler, ReportHandler>();

        builder.Services.AddTransient<IApplicationUserHandler, ApplicationUserHandler>();

        builder.Services.AddTransient<IApplicationUserAddressHandler, ApplicationUserAddressHandler>();

        builder.Services.AddTransient<JwtTokenService>();

        builder.Services.AddTransient<EmailService>();
    }

    public static void AddConfigurationsMvc(this WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();
        builder.Services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); });
        builder.Services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Optimal;
        });
        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; })
            .AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            });
    }
}