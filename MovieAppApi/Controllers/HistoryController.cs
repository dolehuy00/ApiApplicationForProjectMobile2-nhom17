using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppApi.Data;

namespace MovieAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private MovieContext _movieContext;

        public HistoryController(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var histories = await _movieContext.Histories.ToListAsync();
            return Ok(histories);
        }
    }
}
