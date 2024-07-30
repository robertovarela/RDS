namespace RDS.Api.Services;

public class JwtTokenService(IConfiguration configuration)
{
    public string GenerateToken(User user)
    {
        if (user.Email != null)
        {
            var claims = new[]
            {
                new Claim("user_id", user.Id.ToString()), // ID do usuário
                new Claim("email", user.Email), // E-mail do usuário
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email)
            };
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ApiConfiguration.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(240),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        return "InvalidToken";
    }
}   