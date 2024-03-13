using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppApi.Data;

namespace MovieAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchListItemController : ControllerBase
    {
        private MovieContext _movieContext;

        public WatchListItemController(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var watchListItem = await _movieContext.WatchListItems.ToListAsync();
            return Ok(watchListItem);
        }
    }
}
