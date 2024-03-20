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
        private BuildJSON buildJSON;

        public WatchListItemController(MovieContext movieContext)
        {
            _movieContext = movieContext;
            this.buildJSON = new BuildJSON();
        }

        [HttpGet("all/{watchListId}")]
        public async Task<IActionResult> GetAll(int watchListId)
        {
            try
            {
                var allWatchListItems = await _movieContext.WatchListItems
                    .Where(w => w.WatchListId == watchListId)
                    .ToListAsync();
                return Ok(buildJSON.WatchListItemAll(allWatchListItems));
            }
            catch (Exception)
            {
                return BadRequest();
            } 
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNew([FromBody] WatchListItemDTO watchListItemDTO)
        {
            try
            {
                var oldWatchListItem = await _movieContext.WatchListItems
                    .Where(w => w.WatchListId == watchListItemDTO.WatchListId && w.InformationMovie == watchListItemDTO.InformationMovie)
                    .FirstOrDefaultAsync();
                if (oldWatchListItem != null)
                {
                    return BadRequest("Item is exist in playlist");
                }
                var newWatchListItem = new WatchListItem();
                newWatchListItem.WatchListId = watchListItemDTO.WatchListId;
                newWatchListItem.InformationMovie = watchListItemDTO.InformationMovie;
                await _movieContext.WatchListItems.AddAsync(newWatchListItem);
                await _movieContext.SaveChangesAsync();
                return Ok(buildJSON.WatchListItemGet(newWatchListItem));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpGet("check")]
        public async Task<IActionResult> CheckExistInWatchList([FromBody] WatchListItemDTO watchListItemDTO)
        {
            try
            {
                var allWatchLists = await _movieContext.WatchListItems
                    .Where(w => w.WatchListId == watchListItemDTO.WatchListId && w.InformationMovie == watchListItemDTO.InformationMovie)
                    .FirstOrDefaultAsync();
                return Ok(new
                {
                    isExist = allWatchLists != null
                });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        //[HttpPut("edit/{watchListItemId}")]
        //public async Task<IActionResult> Update(int watchListItemId, [FromBody] WatchListItemDTO watchListItemDTO)
        //{
        //    try
        //    {
        //        var oldWatchListItem = await _movieContext.WatchListItems.FirstOrDefaultAsync(w => w.Id == watchListItemId);
        //        if (oldWatchListItem == null)
        //        {
        //            return NotFound();
        //        }
        //        if (watchListItemId != watchListItemDTO.Id)
        //        {
        //            return BadRequest();
        //        }
        //        oldWatchListItem.WatchListId = watchListItemDTO.WatchListId;
        //        await _movieContext.SaveChangesAsync();
        //        return Ok();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        return BadRequest();
        //    }
        //}

        [HttpDelete("delete-one/{watchListItemId}")]
        public async Task<IActionResult> DeleteAWatchListItem(int watchListItemId)
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
        public async Task<IActionResult> DeleteManyWatchListItems([FromBody] int[] watchListItemIds)
        {
            try
            {
                if (watchListItemIds == null || watchListItemIds.Length == 0)
                {
                    return BadRequest("Danh sách id trống.");
                }
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
