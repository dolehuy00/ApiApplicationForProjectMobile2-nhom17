using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.IdentityModel.Tokens;
using MovieAppApi.DTO;
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

        public static GoogleTokenInfo? AcceptGoogleToken(string idToken, IConfiguration _config)
        {
            try
            {
                GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _config["GoogleToken:Audience"] }
                };
                GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(idToken, settings).Result;

                GoogleTokenInfo tokenInfo = new GoogleTokenInfo();
                tokenInfo.Name = payload.Name;
                tokenInfo.Email = payload.Email;
                tokenInfo.Picture = payload.Picture;
                return tokenInfo;
            }
            catch (InvalidJwtException ex)
            {
                Console.WriteLine("ID token không hợp lệ: " + ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi xác minh ID token: " + ex.Message);
                return null;
            }
        }

        public static async Task<FirebaseUserTokenInfo?> AcceptFirebaseAuthenFacebookToken(string idToken)
        {
            try
            {
                if (FirebaseApp.DefaultInstance == null)
                {
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile("Properties/firebase-adminsdk.json"),
                    });
                }
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);

                FirebaseUserTokenInfo tokenInfo = new FirebaseUserTokenInfo();
                tokenInfo.Name = (string)decodedToken.Claims["name"];
                tokenInfo.Picture = (string)decodedToken.Claims["picture"];
                tokenInfo.FirebaseUserId = (string)decodedToken.Claims["user_id"];
                return tokenInfo;
            }
            catch (FirebaseAuthException ex)
            {
                Console.WriteLine("ID token không hợp lệ: " + ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi xác minh ID token: " + ex.Message);
                return null;
            }
        }
    }
}

