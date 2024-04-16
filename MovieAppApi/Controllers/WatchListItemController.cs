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
        private InformationMovieBO _informationMovieBO;
        private WatchListBO _watchListBO;
        private readonly TokenJwtService tokenJwtServ;

        public WatchListItemController(MovieContext movieContext)
        {
            _movieContext = movieContext;
            buildJSON = new BuildJSON();
            tokenJwtServ = new TokenJwtService();
            _informationMovieBO = new InformationMovieBO(movieContext);
            _watchListBO = new WatchListBO(movieContext);
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
                            .Include(w => w.InformationMovie)
                            .ToListAsync();
                    return Ok(buildJSON.WatchListItemAll(allWatchListItems));
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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
                            .Include(w => w.InformationMovie)
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
                            .Include(w => w.InformationMovie)
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
                            .Where(w => w.WatchListId == watchListItemDTO.WatchListId
                                && w.InformationMovie.Tag == watchListItemDTO.InformationMovie.Tag
                                && w.InformationMovie.MovieId == watchListItemDTO.InformationMovie.MovieId)
                            .FirstOrDefaultAsync();
                    if (oldWatchListItem != null)
                    {
                        return BadRequest("Item is exist in playlist");
                    }

                    InformationMovie newInformationMovie = new InformationMovie();
                    newInformationMovie.Title = watchListItemDTO.InformationMovie.Title;
                    newInformationMovie.MovieId = watchListItemDTO.InformationMovie.MovieId;
                    newInformationMovie.ImageLink = watchListItemDTO.InformationMovie.ImageLink;
                    newInformationMovie.Tag = watchListItemDTO.InformationMovie.Tag;
                    newInformationMovie.Durations = watchListItemDTO.InformationMovie.Durations;

                    var newWatchListItem = new WatchListItem();
                    newWatchListItem.WatchListId = watchListItemDTO.WatchListId;
                    newWatchListItem.InformationMovie = await _informationMovieBO.AddOrUpdateInformationMovie(newInformationMovie);

                    await _movieContext.WatchListItems.AddAsync(newWatchListItem);
                    await _movieContext.SaveChangesAsync();
                    await _watchListBO.PlustOneItemCount(newWatchListItem.WatchListId);
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


        [HttpPost("check/{userId}")]
        public async Task<IActionResult> CheckExistInWatchList([FromBody] WatchListItemDTO watchListItemDTO, int userId)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    var allWatchLists = await _movieContext.WatchListItems
                                .Where(w => w.WatchListId == watchListItemDTO.WatchListId
                                    && w.InformationMovie.MovieId == watchListItemDTO.InformationMovie.MovieId
                                    && w.InformationMovie.Tag == watchListItemDTO.InformationMovie.Tag)
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

        [HttpPost("check-all/{userId}")]
        public async Task<IActionResult> CheckExistInAllWatchList([FromBody] InformationMovieDTO informationMovieDTO, int userId)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    var allWatchLists = await _movieContext.WatchListItems
                                .Where(w => w.InformationMovie.MovieId == informationMovieDTO.MovieId
                                    && w.InformationMovie.Tag == informationMovieDTO.Tag).ToListAsync();
                    var inWatchList = allWatchLists.Select(w => w.WatchListId).ToArray();
                    return Ok(new
                    {
                        inWatchList
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



        [HttpDelete("delete-one/{watchListItemId}/{userId}")]
        public async Task<IActionResult> DeleteAWatchListItem(int watchListItemId, int userId)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    var watchListItemToDelete = await _movieContext.WatchListItems
                        .Where(w => w.Id == watchListItemId)
                        .FirstOrDefaultAsync();
                    if (watchListItemToDelete == null)
                    {
                        return NotFound("Không tìm thấy mục để xóa.");
                    }
                    _movieContext.WatchListItems.Remove(watchListItemToDelete);
                    await _movieContext.SaveChangesAsync();
                    await _watchListBO.MinusItemCount(watchListItemToDelete.WatchListId, 1);
                    return Ok(new { success = true });
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
                    var userWatchLists = await _movieContext.WatchListItems
                        .Where(w => watchListItemIds.Contains(w.Id))
                        .ToListAsync();
                    if (userWatchLists.Count == 0)
                    {
                        return NotFound("Không tìm thấy mục để xóa.");
                    }
                    _movieContext.WatchListItems.RemoveRange(userWatchLists);
                    await _movieContext.SaveChangesAsync();
                    await _watchListBO.MinusItemCount(userWatchLists.First().WatchListId, watchListItemIds.Length);
                    return Ok(new { success = true });
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
