namespace RDS.Api.Services;

public class JwtTokenService(IConfiguration configuration, UserManager<User> userManager)
{
    public string GenerateToken(User user, IList<string> roles, string fingerPrint)
    {
        if (user.Email == null) return "InvalidToken";

        var handler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ApiConfiguration.JwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(user, roles, fingerPrint),
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddMinutes(ApiConfiguration.JwtMinutesToExpire),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    public async Task<string> RenewTokenIfNecessary(RefreshTokenRequest refreshTokenRequest)
    {
        var principal = GetPrincipalFromExpiredToken(refreshTokenRequest.Token);

        var fingerPrintClient = refreshTokenRequest.FingerPrint;
        var fingerPrint = principal.Claims.FirstOrDefault(c => c.Type == "FingerPrint")?.Value ?? string.Empty;
        if (fingerPrint != fingerPrintClient) return string.Empty;

        var expiryDateUnix = long.Parse(principal.Claims.First(c => c.Type == JwtRegisteredClaimNames.Exp).Value);
        var expiryDateTimeUtc = DateTimeOffset.FromUnixTimeSeconds(expiryDateUnix).UtcDateTime;
        var currentUtcTime = DateTime.UtcNow;

        if (expiryDateTimeUtc - currentUtcTime > TimeSpan.FromMinutes(ApiConfiguration.JwtMinutesToRefresh))
             return string.Empty;

        var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
            throw new SecurityTokenException("Invalid token: required claims missing");

        var user = await userManager.FindByEmailAsync(email)
                   ?? throw new SecurityTokenException("Invalid token: user not found");

        var roles = await userManager.GetRolesAsync(user);
        return GenerateToken(user, roles, fingerPrint);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ApiConfiguration.JwtKey)),
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            ValidateLifetime = true
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    private static ClaimsIdentity GenerateClaims(User user, IList<string> roles, string fingerPrint)
    {
        var ci = new ClaimsIdentity();
        ci.AddClaim(new Claim("user_id", user.Id.ToString()));
        if (!string.IsNullOrEmpty(user.Email))
        {
            ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            ci.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
            ci.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            ci.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Email));
            ci.AddClaim(new Claim("FingerPrint", fingerPrint));
        }

        foreach (var role in roles)
        {
            ci.AddClaim(new Claim($"custom_role_{role}", role));
        }

        // foreach (var role in roles)
        // {
        //     ci.AddClaim(new Claim(ClaimTypes.Role, role));
        // }

        return ci;
    }


    // private static string GetMacAddress()
    // {
    //     return NetworkInterface
    //         .GetAllNetworkInterfaces()
    //         .Where(nic =>
    //             nic.OperationalStatus == OperationalStatus.Up &&
    //             nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
    //         .Select(nic => nic.GetPhysicalAddress().ToString())
    //         .FirstOrDefault() ?? string.Empty;
    // }

/*
 * 3. Identificador de Android e iOS
Para dispositivos móveis, o Android ID ou Device ID pode ser utilizado, mas cada sistema operacional (Android/iOS) tem sua própria maneira de fornecer identificadores únicos:

Android: Use o Build.SERIAL ou o Settings.Secure.ANDROID_ID.
iOS: Use o IdentifierForVendor.
 */
    // private static string GetDiskSerialNumber()
    // {
    //     string serialNumber = string.Empty;
    //     try
    //     {
    //         ManagementObjectSearcher searcher =
    //             new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_DiskDrive");
    //         foreach (ManagementObject disk in searcher.Get())
    //         {
    //             serialNumber = disk["SerialNumber"].ToString();
    //             break;
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         throw;
    //     }
    //     return serialNumber;
    // }
    //
    //
    // private static string GetCpuId()
    // {
    //     string cpuId = string.Empty;
    //     try
    //     {
    //         ManagementObjectSearcher searcher =
    //             new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor");
    //         foreach (ManagementObject disk in searcher.Get())
    //         {
    //             cpuId = disk["ProcessorId"].ToString();
    //             break;
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         throw;
    //     }
    //     return cpuId;
    // }
    //
    // private static string GenerateDeviceIdentifier()
    // {
    //     var diskSerial = GetDiskSerialNumber();
    //     var cpuId = GetCpuId();
    //     var combinedIdentifier = $"{cpuId}-{diskSerial}";
    //
    //     using (var sha256 = SHA256.Create())
    //     {
    //         var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedIdentifier));
    //         return BitConverter.ToString(hash).Replace("-", "").ToLower();
    //     }
    // }
}