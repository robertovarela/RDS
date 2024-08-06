namespace RDS.Api.Services;

public class JwtTokenService(IConfiguration configuration)
{
    public string GenerateToken(User user)
    {
        if (user.Email != null)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ApiConfiguration.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var claims = new[]
            {
                new Claim("user_id", user.Id.ToString()), // ID do usuário
                new Claim("email", user.Email), // E-mail do usuário
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email)
            };
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
                Subject = GenerateClaims(user)
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
            
            //return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        return "InvalidToken";
    }
    
    private static ClaimsIdentity GenerateClaims(User user)
    {
        var ci = new ClaimsIdentity();

        ci.AddClaim(new Claim("user_id", user.Id.ToString()));
        ci.AddClaim(new Claim("email", user.Email));
        //ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        ci.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
        ci.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        ci.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Email));

        if (user.Roles != null)
            foreach (var role in user.Roles)
                if (role.Name != null)
                    ci.AddClaim(new Claim(ClaimTypes.Role, role.Name));

        return ci;
    }
}   