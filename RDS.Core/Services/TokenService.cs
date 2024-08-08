namespace RDS.Core.Services;

public class TokenService(string jwtKey, string issuer, string audience)
{
    private ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtKey);

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            
            return tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public string? GetUserEmailFromToken(string token)
    {
        var principal = ValidateToken(token);
        if (principal == null) return null;

        var emailClaim = principal.FindFirst("email");
        return emailClaim?.Value;
    }
    public string? GetUserIdFromToken(string token)
    {
        var principal = ValidateToken(token);
        if (principal == null) return null;

        var userIdClaim = principal.FindFirst("user_id");
        return userIdClaim?.Value;
    }
}