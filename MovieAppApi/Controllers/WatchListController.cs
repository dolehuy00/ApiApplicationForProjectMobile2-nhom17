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
    public class WatchListController : ControllerBase
    {
        private MovieContext _movieContext;
        private BuildJSON buildJSON;

        public WatchListController(MovieContext movieContext)
        {
            _movieContext = movieContext;
            this.buildJSON = new BuildJSON();
        }

        [HttpGet("all/{userId}")]
        public async Task<IActionResult> GetAll(int userId)
        {
            try
            {
                var allWatchLists = await _movieContext.WatchLists
                .Where(h => h.UserId == userId)
                .ToListAsync();
                return Ok(buildJSON.WatchListAll(allWatchLists));
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
                var newWatchList = new WatchList();
                newWatchList.UserId = watchListDTO.UserId;
                newWatchList.Title = watchListDTO.Title;
                await _movieContext.WatchLists.AddAsync(newWatchList);
                await _movieContext.SaveChangesAsync();
                return Ok(buildJSON.WatchListGet(newWatchList));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("edit/{watchListId}")]
        public async Task<IActionResult> Update(int watchListId, [FromBody] WatchListDTO watchListDTO)
        {
            try
            {
                var oldWatchList = await _movieContext.WatchLists.FirstOrDefaultAsync(w => w.Id == watchListId);
                if (oldWatchList == null)
                {
                    return NotFound();
                }
                if (watchListId != watchListDTO.Id)
                {
                    return BadRequest();
                }
                oldWatchList.Title = watchListDTO.Title;
                await _movieContext.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }

        [HttpDelete("delete-one/{watchListId}")]
        public async Task<IActionResult> DeleteAWatchList(int watchListId)
        {
            try
            {
                var watchListToDelete = await _movieContext.WatchLists.Where(w => w.Id == watchListId).FirstOrDefaultAsync();
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
        public async Task<IActionResult> DeleteManyWatchLists([FromBody] int[] watchListIds)
        {
            if (watchListIds == null || watchListIds.Length == 0)
            {
                return BadRequest("Danh sách id trống.");
            }
            try
            {
                var userWatchLists = await _movieContext.WatchLists.Where(w => watchListIds.Contains(w.Id)).ToListAsync();
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
