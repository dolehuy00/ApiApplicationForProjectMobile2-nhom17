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
        public async Task<IActionResult> CheckLogin(string email, string password)
        {
            var user = await _movieContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null && password.Equals(user.Password)) 
            {
                return Ok(buildJSON.UserCheckLogin(user));
            }
            return BadRequest("Not match");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            var user = await _movieContext.Users.FirstOrDefaultAsync(u => u.Email == userDTO.Email);
            if (user != null)
            {
                return BadRequest("This email is already in use by another account!");
            }else if(userDTO.Password != userDTO.PasswordConfirm)
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
                return Ok(userDTO);
            }
        }
    }
}
