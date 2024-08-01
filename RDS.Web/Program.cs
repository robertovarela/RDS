using System.Globalization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using RDS.Core.Handlers;
using RDS.Core.Services;
using RDS.Web;
using RDS.Web.Handlers;
using RDS.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
Configuration.jwtKey = builder.Configuration.GetValue<string>("Jwt:Key") 
                          ?? throw new InvalidOperationException("JwtKey is not configured.");
Configuration.issuer = builder.Configuration.GetValue<string>("Jwt:Issuer") 
                              ?? throw new InvalidOperationException("JwtIssuer is not configured.");
Configuration.audience = builder.Configuration.GetValue<string>("Jwt:Audience") 
                          ?? throw new InvalidOperationException("JwtAudience is not configured.");

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<HttpClientService>();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<TokenService>(sp =>
{
    var jwtKey = Configuration.jwtKey;
    var issuer = Configuration.issuer;
    var audience = Configuration.audience;

    return new TokenService(jwtKey, issuer, audience);
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(Configuration.BackendUrl) });

builder.Services.AddTransient<IAccountHandler, AccountHandler>();
builder.Services.AddTransient<IApplicationUserHandler, ApplicationUserHandler>();
builder.Services.AddTransient<IApplicationUserAddressHandler, ApplicationUserAdressHandler>();
builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<IReportHandler, ReportHandler>();

builder.Services.AddScoped<UserStateService>();
builder.Services.AddScoped<LinkUserStateService>();
builder.Services.AddScoped<ManipulateUserStateValuesService>();

builder.Services.AddLocalization();
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

await builder.Build().RunAsync();