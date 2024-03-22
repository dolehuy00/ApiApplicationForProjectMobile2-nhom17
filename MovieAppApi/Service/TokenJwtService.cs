using Microsoft.IdentityModel.Tokens;
using MovieAppApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieAppApi.Service
{
    public class TokenJwtService
    {
        public string GetUserIdFromToken(HttpContext context)
        {
            // Lấy token từ tiêu đề Authorization của HttpContext
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                // Giải mã token thành một đối tượng JwtSecurityToken
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken != null)
                {
                    // Lấy các claims từ token
                    var claims = jwtToken.Claims;

                    // Tìm claim có tên là "UserId" và lấy giá trị của nó
                    var userIdClaim = claims.FirstOrDefault(c => c.Type == "UserId");

                    if (userIdClaim != null)
                    {
                        return userIdClaim.Value;
                    }
                }
            }
            return string.Empty;
        }
        public string GenerateJwtToken(User user, IConfiguration _config)
        {
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"] !),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("Email", user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(10),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
