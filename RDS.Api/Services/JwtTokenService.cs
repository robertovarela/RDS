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
            Expires = DateTime.UtcNow.AddSeconds(260),
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
        if (timeToExpiry > TimeSpan.FromMinutes(5)) return token;
        {
            var userId = principal.Claims.First(c => c.Type == "user_id").Value;
            var email = principal.Claims.First(c => c.Type == "email").Value;
            var user = new User { Id = int.Parse(userId), Email = email };
            var roles = await userManager.GetRolesAsync(user);

            return GenerateToken(user, roles);
        }
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
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ApiConfiguration.JwtKey)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
    private static ClaimsIdentity GenerateClaims(User user, IList<string> roles)
    {
        var ci = new ClaimsIdentity();

        ci.AddClaim(new Claim("user_id", user.Id.ToString()));
        if (user.Email != null)
        {
            ci.AddClaim(new Claim("email", user.Email));
            //ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            ci.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
            ci.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            ci.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Email));
        }

        foreach (var role in roles)
            ci.AddClaim(new Claim(ClaimTypes.Role, role));

        return ci;
    }
}