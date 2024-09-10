namespace RDS.Core.Services;

public class TokenServiceCore(string jwtKey, string issuer, string audience)
{
    private ClaimsPrincipal? ValidateToken(string token, bool validateLifeTime = true)
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
                ValidateLifetime = validateLifeTime,
                ClockSkew = TimeSpan.Zero
            };
            
            return tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
        }
        catch (Exception ex)
        {
            var mensaje = ex.Message;
            return null;
        }
    }

    public string? GetUserEmailFromToken(string token, bool validateLifeTime = true)
    {
        var principal = ValidateToken(token, validateLifeTime);
        if (principal == null) return null;

        var emailClaim = principal.FindFirst("email");
        return emailClaim?.Value;
    }
    
    public string? GetUserIdFromToken(string token, bool validateLifeTime = true)
    {
        var principal = ValidateToken(token, validateLifeTime);
        if (principal == null) return null;

        var userIdClaim = principal.FindFirst("user_id");
        return userIdClaim?.Value;
    }
}