using System.Globalization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using RDS.Web;
using RDS.Web.Handlers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

ConfigurationWeb.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
ConfigurationWeb.JwtKey = builder.Configuration.GetValue<string>("Jwt:Key") 
                          ?? throw new InvalidOperationException("JwtKey is not configured.");
ConfigurationWeb.Issuer = builder.Configuration.GetValue<string>("Jwt:Issuer") 
                              ?? throw new InvalidOperationException("JwtIssuer is not configured.");
ConfigurationWeb.Audience = builder.Configuration.GetValue<string>("Jwt:Audience") 
                          ?? throw new InvalidOperationException("JwtAudience is not configured.");

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<DeviceService>();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 2000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<HttpClientService>();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<TokenServiceCore>(sp =>
{
    var jwtKey = ConfigurationWeb.JwtKey;
    var issuer = ConfigurationWeb.Issuer;
    var audience = ConfigurationWeb.Audience;

    return new TokenServiceCore(jwtKey, issuer, audience);
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(ConfigurationWeb.BackendUrl) });

builder.Services.AddTransient<IApplicationUserHandler, ApplicationUserHandler>();
builder.Services.AddTransient<IApplicationUserAddressHandler, ApplicationUserAddressHandler>();
builder.Services.AddTransient<IApplicationUserTelephoneHandler, ApplicationUserTelephoneHandler>();
builder.Services.AddTransient<IApplicationUserConfigurationHandler, ApplicationUserConfigurationHandler>();
builder.Services.AddTransient<ICompanyHandler, CompanyHandler>();
builder.Services.AddScoped<ICompanyRequestUserRegistrationHandler, CompanyRequestUserRegistrationHandler>();
builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
builder.Services.AddTransient<IReportHandler, ReportHandler>();

builder.Services.AddScoped<UserStateService>();
builder.Services.AddScoped<LinkUserStateService>();
builder.Services.AddScoped<ManipulateUserStateValuesService>();

builder.Services.AddLocalization();
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

await builder.Build().RunAsync();