var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddKerstrel();
builder.AddSecurity();
builder.AddConfigurationsMvc();
builder.AddDataContexts();
builder.AddCrossOrigin();
builder.AddDocumentation();
builder.AddServices();

var app = builder.Build();
app.AppConfiguration();
await app.SeedDatabase();
app.Run();