using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppApi.Data;
using MovieAppApi.DTO;
using MovieAppApi.Models;
using MovieAppApi.Service;

namespace MovieAppApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WatchListItemController : ControllerBase
    {
        private MovieContext _movieContext;
        private BuildJSON buildJSON;
        private readonly TokenJwtService tokenJwtServ;

        public WatchListItemController(MovieContext movieContext)
        {
            _movieContext = movieContext;
            buildJSON = new BuildJSON();
            tokenJwtServ = new TokenJwtService();
        }

        [HttpGet("all/{watchListId}/{userId}")]
        public async Task<IActionResult> GetAll(int watchListId, int userId)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    var allWatchListItems = await _movieContext.WatchListItems
                                        .Where(w => w.WatchListId == watchListId)
                                        .ToListAsync();
                    return Ok(buildJSON.WatchListItemAll(allWatchListItems));
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{limit}/{watchListId}/{userId}")]
        public async Task<IActionResult> GetLimitWatchListItems(int watchListId, int userId, int limit)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    var allWatchListItems = await _movieContext.WatchListItems
                                        .Where(w => w.WatchListId == watchListId)
                                        .Take(limit)
                                        .ToListAsync();
                    return Ok(buildJSON.WatchListItemAll(allWatchListItems));
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{skip}/{take}/{watchListId}/{userId}")]
        public async Task<IActionResult> GetLimitWatchListItems(int watchListId, int userId, int skip, int take)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    var allWatchListItems = await _movieContext.WatchListItems
                                        .Where(w => w.WatchListId == watchListId)
                                        .Skip(skip)
                                        .Take(take)
                                        .ToListAsync();
                    return Ok(buildJSON.WatchListItemAll(allWatchListItems));
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPost("add/{userId}")]
        public async Task<IActionResult> AddNew([FromBody] WatchListItemDTO watchListItemDTO, int userId)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
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
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpGet("check/{userId}")]
        public async Task<IActionResult> CheckExistInWatchList([FromBody] WatchListItemDTO watchListItemDTO, int userId)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    var allWatchLists = await _movieContext.WatchListItems
                                        .Where(w => w.WatchListId == watchListItemDTO.WatchListId && w.InformationMovie == watchListItemDTO.InformationMovie)
                                        .FirstOrDefaultAsync();
                    return Ok(new
                    {
                        isExist = allWatchLists != null
                    });
                }
                else
                {
                    return Unauthorized();
                }

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

        [HttpDelete("delete-one/{watchListItemId}/{userId}")]
        public async Task<IActionResult> DeleteAWatchListItem(int watchListItemId, int userId)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
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
                else
                {
                    return Unauthorized();
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }

        [HttpDelete("delete-many/{userId}")]
        public async Task<IActionResult> DeleteManyWatchListItems([FromBody] int[] watchListItemIds, int userId)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
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
                else
                {
                    return Unauthorized();
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }
    }
}
