using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppApi.Data;
using MovieAppApi.DTO;
using MovieAppApi.Models;

namespace MovieAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private MovieContext _movieContext;
        private BuildJSON buildJSON;

        public UserController(MovieContext movieContext)
        {
            _movieContext = movieContext;
            this.buildJSON = new BuildJSON();
        }

        [HttpPost("login")]
        public async Task<IActionResult> CheckLogin([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var user = await _movieContext.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);
                if (user != null && loginDTO.Password.Equals(user.Password))
                {
                    return Ok(buildJSON.UserCheckLogin(user));
                }
                return BadRequest("Not match");
            }catch (Exception)
            {
                return BadRequest();
            }   
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassDTO changePassDTO)
        {
            try
            {
                var user = await _movieContext.Users.FirstOrDefaultAsync(u => u.Email == changePassDTO.Email);
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
                    user.Password = changePassDTO.NewPassword;
                    await _movieContext.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            try
            {
                var user = await _movieContext.Users.FirstOrDefaultAsync(u => u.Email == userDTO.Email);
                if (user != null)
                {
                    return BadRequest("This email is already in use by another account!");
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
