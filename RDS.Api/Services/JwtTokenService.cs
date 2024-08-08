namespace RDS.Api.Services;

public class JwtTokenService(IConfiguration configuration)
{
    public string GenerateToken(User user, IList<string> roles)
    {
        if (user.Email != null)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ApiConfiguration.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
                Subject = GenerateClaims(user, roles)
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        return "InvalidToken";
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