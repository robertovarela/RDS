namespace RDS.Api.Services;

public class JwtTokenService(IConfiguration configuration, UserManager<User> userManager)
{
    public string GenerateToken(User user, IList<string> roles)
    {
        if (user.Email == null) return "InvalidToken";

        var handler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ApiConfiguration.JwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddMinutes(ApiConfiguration.JwtMinutesToExpire),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            Subject = GenerateClaims(user, roles)
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    public async Task<string> RenewTokenIfNecessary(string token)
    {
        var principal = GetPrincipalFromExpiredToken(token);
        var expiryDateUnix = long.Parse(principal.Claims.First(c => c.Type == JwtRegisteredClaimNames.Exp).Value);

        var expiryDateTimeUtc = DateTimeOffset.FromUnixTimeSeconds(expiryDateUnix).UtcDateTime;
        var currentUtcTime = DateTime.UtcNow;

        var timeToExpiry = expiryDateTimeUtc - currentUtcTime;
        if (timeToExpiry > TimeSpan.FromMinutes(ApiConfiguration.JwtMinutesToRefresh)) return string.Empty;

        var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
        {
            throw new SecurityTokenException("Invalid token: required claims missing");
        }

        var user = await userManager.FindByEmailAsync(email) ??
                   throw new SecurityTokenException("Invalid token: user not found");
        var roles = await userManager.GetRolesAsync(user);
        return GenerateToken(user, roles);
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

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
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

    private static ClaimsIdentity GenerateClaims(User user, IList<string> roles)
    {
        var ci = new ClaimsIdentity();
        ci.AddClaim(new Claim("user_id", user.Id.ToString()));
        if (!string.IsNullOrEmpty(user.Email))
        {
            ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            ci.AddClaim(new Claim(ClaimTypes.GivenName, user.Name ?? string.Empty));
            ci.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            ci.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Email));
        }

        foreach (var role in roles)
            ci.AddClaim(new Claim(ClaimTypes.Role, role));

        return ci;
    }
}