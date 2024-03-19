using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppApi.Data;
using MovieAppApi.DTO;
using MovieAppApi.Models;
using NuGet.Packaging;

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

        [HttpGet("all/{userId}")]
        public async Task<IActionResult> GetAll(int userId)
        {
            try
            {
                var allWatchLists = await _movieContext.WatchLists
                .Where(h => h.UserId == userId)
                .ToListAsync();
                return Ok(allWatchLists);
            }
            catch (Exception)
            {
                return BadRequest();
            } 
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNew(WatchListDTO watchListDTO)
        {
            try
            {
                var preWatchList = new WatchList();
                preWatchList.UserId = watchListDTO.UserId;
                preWatchList.Title = watchListDTO.Title;
                var newWatchList = await _movieContext.WatchLists.AddAsync(preWatchList);
                return Ok(newWatchList);
            }
            catch (Exception)
            {
                return BadRequest();
            } 
        }

        [HttpPut("edit/{watchListId}")]
        public async Task<IActionResult> Update(int watchListId, [FromBody] WatchListDTO watchListDTO)
        {
            if (watchListId != watchListDTO.Id)
            {
                return BadRequest();
            }
            var preWatchList = new WatchList();
            preWatchList.UserId = watchListDTO.UserId;
            preWatchList.Title = watchListDTO.Title;
            _movieContext.Entry(preWatchList).State = EntityState.Modified;
            try
            {
                await _movieContext.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_movieContext.WatchLists.Any(w => w.Id == watchListId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        [HttpDelete("delete-one/{watchListId}")]
        public async Task<IActionResult> DeleteAWatchList(int userId, int watchListId)
        {
            try
            {
                var watchListToDelete = await _movieContext.WatchLists.Where(w => w.UserId == userId && w.Id == watchListId).FirstOrDefaultAsync();
                if (watchListToDelete == null)
                {
                    return NotFound("Không tìm thấy danh sách để xóa.");
                }
                _movieContext.WatchLists.Remove(watchListToDelete);
                await _movieContext.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }
        [HttpDelete("delete-many")]
        public async Task<IActionResult> DeleteManyWatchLists(int userId, [FromBody] int[] watchListIds)
        {
            if (watchListIds == null || watchListIds.Length == 0)
            {
                return BadRequest("Danh sách id trống.");
            }
            try
            {
                var userWatchLists = await _movieContext.WatchLists.Where(w => watchListIds.Contains(w.Id) && w.UserId == userId).ToListAsync();
                if (userWatchLists.Count == 0)
                {
                    return NotFound("Không tìm thấy danh sách để xóa.");
                }
                _movieContext.WatchLists.RemoveRange(userWatchLists);
                await _movieContext.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }
    }

}
