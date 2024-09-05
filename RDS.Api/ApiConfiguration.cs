namespace RDS.Api;

public static class ApiConfiguration
{
    public const string CorsPolicyName = "wasm";
    public static string StripeApiKey { get; set; } = string.Empty;
    
    public static string JwtKey = "ZmVkYWY3ZDg4NjNiNDhlMTk3YjkyODdkNDkyYjcwOGU=";
    public static string JwtIssuer = "https://localhost:5001";
    public static string JwtAudience = "https://localhost:5001";
    public static readonly int JwtMinutesToRefresh = 450;
    public static readonly int JwtMinutesToExpire = 500;
    public static string ApiKeyName = "api_key";
    public static string ApiKey = "RDS_api_IlTevUM/z0ey3NwCV/unrSWg==";
    public static SmtpConfiguration Smtp = new();

    public const int PageSize = 25;

    public class SmtpConfiguration
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; } = 25;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}