using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppApi.Data;

namespace MovieAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchListController : ControllerBase
    {
        private MovieContext _movieContext;

        public WatchListController(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var watchlist = await _movieContext.WatchList.ToListAsync();
            return Ok(watchlist);
        }
    }

}
