using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MovieAppApi.Data;
using MovieAppApi.DTO;
using MovieAppApi.Models;
using MovieAppApi.Service;

namespace MovieAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private MovieContext _movieContext;
        private BuildJSON buildJSON;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _config;
        private readonly TokenJwtService tokenJwtServ;

        public UserController(MovieContext movieContext, IMemoryCache cache, IConfiguration config)
        {
            _config = config;
            _movieContext = movieContext;
            _cache = cache;
            buildJSON = new BuildJSON();
            tokenJwtServ = new TokenJwtService();
        }

        [HttpPost("login")]
        public async Task<IActionResult> CheckLogin([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var user = await _movieContext.Users
                    .FirstOrDefaultAsync(u => u.Email == loginDTO.Email &&
                                              u.TagSocialNetwork == null);
                if (user != null && loginDTO.Password.Equals(user.Password))
                {
                    var token = tokenJwtServ.GenerateJwtToken(user, _config);
                    return Ok(buildJSON.UserCheckLogin(user, token));
                }
                return Unauthorized("Not match");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login-google")]
        public async Task<IActionResult> CheckLoginWithGoogle([FromBody] string googleToken)
        {
            try
            {
                GoogleTokenInfo? googleTokenInfo = TokenJwtService.AcceptGoogleToken(googleToken, _config);
                if (googleTokenInfo != null)
                {
                    var user = await _movieContext.Users
                        .FirstOrDefaultAsync(u => u.SubUserId == googleTokenInfo.Email &&
                                                  u.TagSocialNetwork == "GOOGLE");
                    if (user != null)
                    {
                        user.Avatar = googleTokenInfo.Picture;
                        user.Name = googleTokenInfo.Name;
                        await _movieContext.SaveChangesAsync();
                        var token = tokenJwtServ.GenerateJwtToken(user, _config);
                        return Ok(buildJSON.UserCheckLogin(user, token));
                    }
                    else
                    {
                        User newUser = new User();
                        newUser.Name = googleTokenInfo.Name;
                        newUser.Email = googleTokenInfo.Email;
                        newUser.Password = RandomStringGenerator.GenerateRandomString(30);
                        newUser.Avatar = googleTokenInfo.Picture;
                        newUser.SubUserId = googleTokenInfo.Email;
                        newUser.TagSocialNetwork = "GOOGLE";
                        _movieContext.Users.Add(newUser);
                        await _movieContext.SaveChangesAsync();
                        var token = tokenJwtServ.GenerateJwtToken(newUser, _config);
                        return Ok(buildJSON.UserCheckLogin(newUser, token));
                    }
                }
                else
                {
                    return Unauthorized("Can't signin");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "\n" + e.StackTrace + "\n \n" + e.InnerException);
            }
        }

        [HttpPost("login-facebook")]
        public async Task<IActionResult> CheckLoginWithFacebook([FromBody] string firebaseToken)
        {
            try
            {
                FirebaseUserTokenInfo? userTokenInfo = await TokenJwtService.AcceptFirebaseAuthenFacebookToken(firebaseToken);
                if (userTokenInfo != null)
                {
                    var user = await _movieContext.Users
                        .FirstOrDefaultAsync(u => u.SubUserId == userTokenInfo.FirebaseUserId &&
                                                  u.TagSocialNetwork == "FACEBOOK");
                    if (user != null)
                    {
                        user.Avatar = userTokenInfo.Picture;
                        user.Name = userTokenInfo.Name;
                        user.SubUserId = userTokenInfo.FirebaseUserId;
                        await _movieContext.SaveChangesAsync();
                        var token = tokenJwtServ.GenerateJwtToken(user, _config);
                        return Ok(buildJSON.UserCheckLogin(user, token));
                    }
                    else
                    {
                        User newUser = new User();
                        newUser.Name = userTokenInfo.Name;
                        newUser.Avatar = userTokenInfo.Picture;
                        newUser.Email = userTokenInfo.FirebaseUserId;
                        newUser.Password = RandomStringGenerator.GenerateRandomString(30);
                        newUser.SubUserId = userTokenInfo.FirebaseUserId;
                        newUser.TagSocialNetwork = "FACEBOOK";
                        _movieContext.Users.Add(newUser);
                        await _movieContext.SaveChangesAsync();
                        var token = tokenJwtServ.GenerateJwtToken(newUser, _config);
                        return Ok(buildJSON.UserCheckLogin(newUser, token));
                    }
                }
                else
                {
                    return Unauthorized("Can't signin");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "\n" + e.StackTrace + "\n \n" + e.InnerException);
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassDTO changePassDTO)
        {
            try
            {

                var user = await _movieContext.Users
                    .FirstOrDefaultAsync(u => u.Email == changePassDTO.Email &&
                                              u.TagSocialNetwork == null);
                if (user == null)
                {
                    return NotFound("This account is'nt exist!");
                }
                else if (changePassDTO.NewPassword != changePassDTO.PasswordConfirm)
                {
                    return BadRequest("Password confirm is not the same password");
                }
                else if (changePassDTO.OldPassword != user.Password)
                {
                    return BadRequest("Old password not match!");
                }
                else
                {
                    var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                    if (int.Parse(userIdInToken) == user.Id)
                    {
                        user.Password = changePassDTO.NewPassword;
                        await _movieContext.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {
                        return Unauthorized();
                    }

                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPost("change-info")]
        public async Task<IActionResult> ChangeAvatar([FromBody] ChangeInfoDTO changeInfoDTO)
        {
            try
            {

                var user = await _movieContext.Users
                    .FirstOrDefaultAsync(u => u.Email == changeInfoDTO.Email &&
                                              u.TagSocialNetwork == null);
                if (user == null)
                {
                    return NotFound("This account is'nt exist!");
                }
                else
                {
                    var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                    if (int.Parse(userIdInToken) == user.Id)
                    {
                        if (changeInfoDTO.Avatar != null)
                        {
                            user.Avatar = changeInfoDTO.Avatar;
                        }
                        if (changeInfoDTO.Name != null)
                        {
                            user.Name = changeInfoDTO.Name;
                        }
                        await _movieContext.SaveChangesAsync();
                        return Ok(buildJSON.UserCheckLogin(user, ""));
                    }
                    else
                    {
                        return Unauthorized();
                    }

                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPost("forgot-password-request")]
        public async Task<IActionResult> ForgotPasswordRequest([FromBody] ForgotDTO forgotDTO)
        {
            try
            {
                var existUser = await _movieContext.Users
                    .Where(u => u.Email == forgotDTO.Email && u.TagSocialNetwork == null).FirstOrDefaultAsync() != null;
                if (existUser)
                {
                    var code = _cache.Get<Code>(forgotDTO.Email);
                    if (code != null)
                    {
                        if (DateTimeOffset.UtcNow.AddMinutes(4) < code.DeadTime)
                        {
                            return BadRequest("Too many request, please wait 1 minute since the last successful request");
                        }
                    }
                    int randomCode = new Random().Next(100100, 999999);
                    _cache.Set(forgotDTO.Email, new Code(randomCode, DateTimeOffset.UtcNow.AddMinutes(5)), TimeSpan.FromMinutes(5));
                    await new PasswordResetService().SendPasswordResetEmail(forgotDTO.Email, randomCode);
                }
                var responseData = new { existUser = existUser };
                return Ok(responseData);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("forgot-password-checkcode")]
        public IActionResult ForgotPasswordCheckCode([FromBody] ForgotDTO forgotDTO)
        {
            try
            {
                var code = _cache.Get<Code>(forgotDTO.Email);
                var codeMatch = code != null && code.CodeNumber.Equals(forgotDTO.Code);
                var responseData = new
                {
                    codeMatch = codeMatch
                };
                return Ok(responseData);
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpPost("forgot-password-change")]
        public async Task<IActionResult> ForgotPasswordChange([FromBody] ForgotDTO forgotDTO)
        {
            try
            {
                if (forgotDTO.Password != forgotDTO.PasswordConfirm)
                {
                    return BadRequest("Password confirm is not the same password");
                }
                var user = await _movieContext.Users
                    .Where(u => u.Email == forgotDTO.Email && u.TagSocialNetwork == null).FirstOrDefaultAsync();
                var code = _cache.Get<Code>(forgotDTO.Email);
                var changeSuccess = false;
                if (user != null && code != null && code.CodeNumber.Equals(forgotDTO.Code))
                {
                    user.Password = forgotDTO.Password;
                    changeSuccess = await _movieContext.SaveChangesAsync() == 1;
                }
                var responseData = new
                {
                    changeSuccess = changeSuccess
                };
                return Ok(responseData);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            try
            {
                var user = await _movieContext.Users
                    .FirstOrDefaultAsync(u => u.Email == userDTO.Email && u.TagSocialNetwork == null);
                if (user != null)
                {
                    return BadRequest("This email is already in use by another account!");
                }
                else if (userDTO.Email.Length < 4)
                {
                    return BadRequest("Invalid email!");
                }
                else if (userDTO.Password.Length < 6)
                {
                    return BadRequest("Password not long!");
                }
                else if (userDTO.Password != userDTO.PasswordConfirm)
                {
                    return BadRequest("Password confirm is not the same password");
                }
                else
                {
                    User newUser = new User();
                    newUser.Email = userDTO.Email;
                    newUser.Name = userDTO.Name;
                    newUser.Password = userDTO.Password;
                    await _movieContext.Users.AddAsync(newUser);
                    await _movieContext.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
