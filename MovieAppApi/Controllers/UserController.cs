using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppApi.Data;

namespace MovieAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private MovieContext _movieContext;

        public UserController(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _movieContext.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _movieContext.Users.FirstOrDefaultAsync(p => p.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
