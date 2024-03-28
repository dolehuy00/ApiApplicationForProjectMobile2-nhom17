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
    public class HistoryController : ControllerBase
    {
        private MovieContext _movieContext;
        private readonly TokenJwtService tokenJwtServ;
        private InformationMovieBO _informationMovieBO;
        private BuildJSON buildJSON;

        public HistoryController(MovieContext movieContext)
        {
            _movieContext = movieContext;
            tokenJwtServ = new TokenJwtService();
            _informationMovieBO = new InformationMovieBO(movieContext);
            buildJSON = new BuildJSON();
        }

        [HttpGet("all/{userId}")]
        public async Task<IActionResult> GetAllHistory(int userId)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    var allHistories = await _movieContext.Histories
                        .Where(h => h.UserId == userId)
                        .Include(h => h.InformationMovie)
                        .OrderByDescending(h => h.WatchedDate)
                        .ToListAsync();
                    return Ok(buildJSON.HistoryItemAll(allHistories));
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


        [HttpGet("{limit}/{userId}")]
        public async Task<IActionResult> GetLimitNewestHistory(int userId, int limit)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    var latestHistories = await _movieContext.Histories
                        .Where(h => h.UserId == userId)
                        .OrderByDescending(h => h.WatchedDate)
                        .Take(limit)
                        .Include(h => h.InformationMovie)
                        .ToListAsync();
                    return Ok(buildJSON.HistoryItemAll(latestHistories));
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
        public async Task<IActionResult> GetLimitNewestHistory(int userId, int skip, int take)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    var latestHistories = await _movieContext.Histories
                        .Where(h => h.UserId == userId)
                        .OrderByDescending(h => h.WatchedDate)
                        .Skip(skip)
                        .Take(take)
                        .Include(h => h.InformationMovie)
                        .ToListAsync();
                    return Ok(buildJSON.HistoryItemAll(latestHistories));
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
        public async Task<IActionResult> AddNewHistory([FromBody] HistoryDTO historyDTO)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == historyDTO.UserId)
                {
                    var oldHistory = await _movieContext.Histories
                        .Where(h => h.InformationMovie.MovieId == historyDTO.InformationMovie.MovieId
                        && h.UserId == historyDTO.UserId)
                        .Include(h => h.InformationMovie)
                        .FirstOrDefaultAsync();
                    InformationMovie newInformationMovie = new InformationMovie();
                    newInformationMovie.Title = historyDTO.InformationMovie.Title;
                    newInformationMovie.MovieId = historyDTO.InformationMovie.MovieId;
                    newInformationMovie.ImageLink = historyDTO.InformationMovie.ImageLink;
                    newInformationMovie.Tag = historyDTO.InformationMovie.Tag;
                    newInformationMovie.Durations = historyDTO.InformationMovie.Durations;
                    if (oldHistory == null)
                    {
                        var newHistory = new History();
                        newHistory.UserId = historyDTO.UserId;
                        newHistory.WatchedDate = historyDTO.WatchedDate;
                        newHistory.SecondsCount = historyDTO.SecondsCount;
                        newHistory.InformationMovie = await _informationMovieBO.AddOrUpdateInformationMovie(newInformationMovie);

                        await _movieContext.Histories.AddAsync(newHistory);
                        await _movieContext.SaveChangesAsync();
                        return Ok(buildJSON.HistoryGet(newHistory));
                    }
                    else
                    {
                        if (oldHistory.InformationMovie.Title != newInformationMovie.Title ||
                           oldHistory.InformationMovie.Durations != newInformationMovie.Durations ||
                           oldHistory.InformationMovie.ImageLink != newInformationMovie.ImageLink)
                        {
                            oldHistory.InformationMovie = await _informationMovieBO.AddOrUpdateInformationMovie(newInformationMovie);
                        }
                        oldHistory.WatchedDate = historyDTO.WatchedDate;
                        oldHistory.SecondsCount = historyDTO.SecondsCount;
                        await _movieContext.SaveChangesAsync();
                        return Ok(buildJSON.HistoryGet(oldHistory));
                    }
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

        [HttpDelete("delete-all/{userId}")]
        public async Task<IActionResult> DeleteAllHistory(int userId)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    var userHistories = await _movieContext.Histories
                        .Where(h => h.UserId == userId)
                        .ToListAsync();
                    if (userHistories.Count == 0)
                    {
                        return NotFound("Không tìm thấy lịch sử để xóa.");
                    }
                    _movieContext.Histories.RemoveRange(userHistories);
                    await _movieContext.SaveChangesAsync();
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

        [HttpDelete("delete-one/{historyId}/{userId}")]
        public async Task<IActionResult> DeleteAHistory(int userId, int historyId)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    var historyToDelete = await _movieContext.Histories
                        .Where(h => h.UserId == userId && h.Id == historyId)
                        .FirstOrDefaultAsync();
                    if (historyToDelete == null)
                    {
                        return NotFound("Không tìm thấy lịch sử để xóa.");
                    }
                    _movieContext.Histories.Remove(historyToDelete);
                    await _movieContext.SaveChangesAsync();
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
        public async Task<IActionResult> DeleteManyHistories(int userId, [FromBody] int[] historyIds)
        {
            if (historyIds == null || historyIds.Length == 0)
            {
                return BadRequest("Danh sách id trống.");
            }
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    var userHistories = await _movieContext.Histories
                        .Where(w => historyIds.Contains(w.Id) && w.UserId == userId)
                        .ToListAsync();
                    if (userHistories.Count == 0)
                    {
                        return NotFound("Không tìm thấy danh sách để xóa.");
                    }
                    _movieContext.Histories.RemoveRange(userHistories);
                    await _movieContext.SaveChangesAsync();
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

        [HttpDelete("delete-last-hour/{userId}")]
        public async Task<IActionResult> DeleteHistoryPreviousOneHour(int userId)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    DateTime oneHourAgo = DateTime.UtcNow.AddHours(-1);
                    var userHistories = await _movieContext.Histories
                        .Where(h => h.UserId == userId && h.WatchedDate >= oneHourAgo)
                        .ToListAsync();
                    if (userHistories.Count == 0)
                    {
                        return NotFound("Không tìm thấy lịch sử để xóa.");
                    }
                    _movieContext.Histories.RemoveRange(userHistories);
                    await _movieContext.SaveChangesAsync();
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
        [HttpDelete("delete-last-day/{userId}")]
        public async Task<IActionResult> DeleteHistoryPreviousOneDay(int userId)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    DateTime oneDayAgo = DateTime.UtcNow.AddDays(-1);
                    var userHistories = await _movieContext.Histories
                        .Where(h => h.UserId == userId && h.WatchedDate >= oneDayAgo)
                        .ToListAsync();
                    if (userHistories.Count == 0)
                    {
                        return NotFound("Không tìm thấy lịch sử để xóa.");
                    }
                    _movieContext.Histories.RemoveRange(userHistories);
                    await _movieContext.SaveChangesAsync();
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
        [HttpDelete("delete-last-week/{userId}")]
        public async Task<IActionResult> DeleteHistoryPreviousOneWeek(int userId)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    DateTime oneWeekAgo = DateTime.UtcNow.AddDays(-7);
                    var userHistories = await _movieContext.Histories
                        .Where(h => h.UserId == userId && h.WatchedDate >= oneWeekAgo)
                        .ToListAsync();
                    if (userHistories.Count == 0)
                    {
                        return NotFound("Không tìm thấy lịch sử để xóa.");
                    }
                    _movieContext.Histories.RemoveRange(userHistories);
                    await _movieContext.SaveChangesAsync();
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
        [HttpDelete("delete-last-month/{userId}")]
        public async Task<IActionResult> DeleteHistoryPreviousOneMonth(int userId)
        {
            try
            {
                var userIdInToken = tokenJwtServ.GetUserIdFromToken(HttpContext);
                if (int.Parse(userIdInToken) == userId)
                {
                    DateTime oneMonthAgo = DateTime.UtcNow.AddMonths(-1);
                    var userHistories = await _movieContext.Histories
                        .Where(h => h.UserId == userId && h.WatchedDate >= oneMonthAgo)
                        .ToListAsync();
                    if (userHistories.Count == 0)
                    {
                        return NotFound("Không tìm thấy lịch sử để xóa.");
                    }
                    _movieContext.Histories.RemoveRange(userHistories);
                    await _movieContext.SaveChangesAsync();
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
