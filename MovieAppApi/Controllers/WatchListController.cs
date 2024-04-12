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
    public class WatchListController : ControllerBase
    {
        private MovieContext _movieContext;
        private BuildJSON buildJSON;
        private readonly TokenJwtService tokenJwtServ;
        private InformationMovieBO _informationMovieBO;

        public WatchListController(MovieContext movieContext)
        {
            _movieContext = movieContext;
            buildJSON = new BuildJSON();
            tokenJwtServ = new TokenJwtService();
            _informationMovieBO = new InformationMovieBO(movieContext);
        }

        [HttpGet("all/{userId}")]
        public async Task<IActionResult> GetAll(int userId)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    var allWatchLists = await _movieContext.WatchLists
                                    .Where(h => h.UserId == userId)
                                    .ToListAsync();
                    return Ok(buildJSON.WatchListAll(allWatchLists));
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

        [HttpGet("{limit}/{userId}")]
        public async Task<IActionResult> GetLimitWatchLists(int userId, int limit)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    var allWatchLists = await _movieContext.WatchLists
                                    .Where(h => h.UserId == userId)
                                    .Take(limit)
                                    .ToListAsync();
                    return Ok(buildJSON.WatchListAll(allWatchLists));
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

        [HttpGet("{skip}/{take}/{userId}")]
        public async Task<IActionResult> GetLimitWatchLists(int userId, int skip, int take)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    var allWatchLists = await _movieContext.WatchLists
                                    .Where(h => h.UserId == userId)
                                    .Skip(skip)
                                    .Take(take)
                                    .ToListAsync();
                    return Ok(buildJSON.WatchListAll(allWatchLists));
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


        [HttpPost("add")]
        public async Task<IActionResult> AddNew(WatchListDTO watchListDTO)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == watchListDTO.UserId)
                {
                    var newWatchList = new WatchList();
                    newWatchList.UserId = watchListDTO.UserId;
                    newWatchList.Title = watchListDTO.Title;
                    await _movieContext.WatchLists.AddAsync(newWatchList);
                    await _movieContext.SaveChangesAsync();
                    return Ok(buildJSON.WatchListGet(newWatchList));
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

        [HttpPost("add-to-new-watchlist")]
        public async Task<IActionResult> AddToNewWatchlist(WatchListDTO watchListDTO)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == watchListDTO.UserId)
                {
                    var newWatchList = new WatchList();
                    newWatchList.UserId = watchListDTO.UserId;
                    newWatchList.Title = watchListDTO.Title;
                    newWatchList.ItemCount = 1;
                    await _movieContext.WatchLists.AddAsync(newWatchList);
                    await _movieContext.SaveChangesAsync();
                    if (watchListDTO.item != null)
                    {
                        InformationMovie newInformationMovie = new InformationMovie();
                        newInformationMovie.Title = watchListDTO.item.InformationMovie.Title;
                        newInformationMovie.MovieId = watchListDTO.item.InformationMovie.MovieId;
                        newInformationMovie.ImageLink = watchListDTO.item.InformationMovie.ImageLink;
                        newInformationMovie.Tag = watchListDTO.item.InformationMovie.Tag;
                        newInformationMovie.Durations = watchListDTO.item.InformationMovie.Durations;

                        var newWatchListItem = new WatchListItem();
                        newWatchListItem.WatchListId = newWatchList.Id;
                        newWatchListItem.InformationMovie = await _informationMovieBO.AddOrUpdateInformationMovie(newInformationMovie);

                        await _movieContext.WatchListItems.AddAsync(newWatchListItem);
                        await _movieContext.SaveChangesAsync();
                    }

                    return Ok(buildJSON.WatchListGet(newWatchList));
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
        }

        [HttpPut("edit/{watchListId}")]
        public async Task<IActionResult> Update(int watchListId, [FromBody] WatchListDTO watchListDTO)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == watchListDTO.UserId)
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

        [HttpDelete("delete-one/{watchListId}/{userId}")]
        public async Task<IActionResult> DeleteAWatchList(int watchListId, int userId)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
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
        public async Task<IActionResult> DeleteManyWatchLists([FromBody] int[] watchListIds, int userId)
        {
            if (watchListIds == null || watchListIds.Length == 0)
            {
                return BadRequest("Danh sách id trống.");
            }
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
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
