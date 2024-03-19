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
    public class WatchListItemController : ControllerBase
    {
        private MovieContext _movieContext;

        public WatchListItemController(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        [HttpGet("all/{watchListId}")]
        public async Task<IActionResult> GetAll(int watchListId)
        {
            var allWatchLists = await _movieContext.WatchListItems
                .Where(w => w.WatchListId == watchListId)
                .ToListAsync();
            return Ok(allWatchLists);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNew(int userId, WatchListItemDTO watchListItemDTO)
        {
            var preWatchListItem = new WatchListItem();
            preWatchListItem.WatchListId = watchListItemDTO.WatchListId;
            preWatchListItem.InformationMovie = watchListItemDTO.InformationMovie;
            var newWatchListItem = await _movieContext.WatchListItems.AddAsync(preWatchListItem);
            return Ok(newWatchListItem);
        }

        [HttpPut("edit/{watchListItemId}")]
        public async Task<IActionResult> Update(int watchListItemId, [FromBody] WatchListItemDTO watchListItemDTO)
        {
            if (watchListItemId != watchListItemDTO.Id)
            {
                return BadRequest();
            }
            var preWatchList = new WatchList();
            preWatchList.UserId = watchListItemDTO.WatchListId;
            preWatchList.Title = watchListItemDTO.InformationMovie;
            _movieContext.Entry(preWatchList).State = EntityState.Modified;
            try
            {
                await _movieContext.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_movieContext.WatchListItems.Any(w => w.Id == watchListItemId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        [HttpDelete("delete-one/{watchListItemId}")]
        public async Task<IActionResult> DeleteAWatchListItem(int userId, int watchListItemId)
        {
            try
            {
                var watchListItemToDelete = await _movieContext.WatchListItems.Where(w => w.Id == watchListItemId).FirstOrDefaultAsync();
                if (watchListItemToDelete == null)
                {
                    return NotFound("Không tìm thấy mục để xóa.");
                }
                _movieContext.WatchListItems.Remove(watchListItemToDelete);
                await _movieContext.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }
        [HttpDelete("delete-many")]
        public async Task<IActionResult> DeleteManyWatchListItems(int userId, [FromBody] int[] watchListItemIds)
        {
            if (watchListItemIds == null || watchListItemIds.Length == 0)
            {
                return BadRequest("Danh sách id trống.");
            }
            try
            {
                var userWatchLists = await _movieContext.WatchListItems.Where(w => watchListItemIds.Contains(w.Id)).ToListAsync();
                if (userWatchLists.Count == 0)
                {
                    return NotFound("Không tìm thấy mục để xóa.");
                }
                _movieContext.WatchListItems.RemoveRange(userWatchLists);
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
