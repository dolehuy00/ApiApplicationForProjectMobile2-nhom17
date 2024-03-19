using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppApi.Data;
using MovieAppApi.DTO;
using MovieAppApi.Models;

namespace MovieAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private MovieContext _movieContext;

        public HistoryController(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        [HttpGet("all/{userId}")]
        public async Task<IActionResult> GetAllHistory(int userId)
        {
            try
            {
                var allHistories = await _movieContext.Histories
                    .Where(h => h.UserId == userId)
                    .OrderByDescending(h => h.WatchedDate)
                    .ToListAsync();
                return Ok(allHistories);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            
        }

        [HttpGet("{limit}/{userId}")]
        public async Task<IActionResult> GetLimitNewestHistory(int userId, int limit)
        {
            try
            {
                var latestHistories = await _movieContext.Histories
                    .Where(h => h.UserId == userId)
                    .OrderByDescending(h => h.WatchedDate)
                    .Take(limit)
                    .ToListAsync();
                return Ok(latestHistories);
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
                var latestHistories = await _movieContext.Histories
                    .Where(h => h.UserId == userId)
                    .OrderByDescending(h => h.WatchedDate)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
                return Ok(latestHistories);
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
                var oldHistory = await _movieContext.Histories.Where(h => h.InformationMovie == historyDTO.InformationMovie).FirstOrDefaultAsync();

                if (oldHistory == null)
                {
                    var newHistory = new History();
                    newHistory.UserId = historyDTO.UserId;
                    newHistory.InformationMovie = historyDTO.InformationMovie;
                    newHistory.WatchedDate = historyDTO.WatchedDate;
                    newHistory.SecondsCount = historyDTO.SecondsCount;
                    await _movieContext.Histories.AddAsync(newHistory);
                    await _movieContext.SaveChangesAsync();
                    return Ok(newHistory);
                }
                else
                {
                    oldHistory.WatchedDate = historyDTO.WatchedDate;
                    oldHistory.SecondsCount = historyDTO.SecondsCount;
                    await _movieContext.SaveChangesAsync();
                    return Ok(oldHistory);
                }
                
            }
            catch (Exception)
            {
                return BadRequest();
            } 
        }

        [HttpDelete("delete-all/{userId}")]
        public async Task<IActionResult> DeleteAllHistory(int userId)
        {
            //Xac thuc user
            try
            {
                var userHistories = await _movieContext.Histories.Where(h => h.UserId == userId).ToListAsync();
                if (userHistories.Count == 0)
                {
                    return NotFound("Không tìm thấy lịch sử để xóa.");
                }
                _movieContext.Histories.RemoveRange(userHistories);
                await _movieContext.SaveChangesAsync();
                return Ok();
            }catch(DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }

        [HttpDelete("delete-one/{historyId}/{userId}")]
        public async Task<IActionResult> DeleteAHistory(int userId, int historyId)
        {
            try
            {
                var historyToDelete = await _movieContext.Histories.Where(h => h.UserId == userId && h.Id == historyId).FirstOrDefaultAsync();
                if (historyToDelete == null)
                {
                    return NotFound("Không tìm thấy lịch sử để xóa.");
                }
                _movieContext.Histories.Remove(historyToDelete);
                await _movieContext.SaveChangesAsync();
                return Ok();
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
                var userHistories = await _movieContext.Histories.Where(w => historyIds.Contains(w.Id) && w.UserId == userId).ToListAsync();
                if (userHistories.Count == 0)
                {
                    return NotFound("Không tìm thấy danh sách để xóa.");
                }
                _movieContext.Histories.RemoveRange(userHistories);
                await _movieContext.SaveChangesAsync();
                return Ok();
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
                DateTime oneHourAgo = DateTime.UtcNow.AddHours(-1);
                var userHistories = await _movieContext.Histories.Where(h => h.UserId == userId && h.WatchedDate >= oneHourAgo).ToListAsync();
                if (userHistories.Count == 0)
                {
                    return NotFound("Không tìm thấy lịch sử để xóa.");
                }
                _movieContext.Histories.RemoveRange(userHistories);
                await _movieContext.SaveChangesAsync();
                return Ok();
            }
            catch(DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }
        [HttpDelete("delete-last-day/{userId}")]
        public async Task<IActionResult> DeleteHistoryPreviousOneDay(int userId)
        {
            try
            {
                DateTime oneDayAgo = DateTime.UtcNow.AddDays(-1);
                var userHistories = await _movieContext.Histories.Where(h => h.UserId == userId && h.WatchedDate >= oneDayAgo).ToListAsync();
                if (userHistories.Count == 0)
                {
                    return NotFound("Không tìm thấy lịch sử để xóa.");
                }
                _movieContext.Histories.RemoveRange(userHistories);
                await _movieContext.SaveChangesAsync();
                return Ok();
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
                DateTime oneWeekAgo = DateTime.UtcNow.AddDays(-7);
                var userHistories = await _movieContext.Histories.Where(h => h.UserId == userId && h.WatchedDate >= oneWeekAgo).ToListAsync();
                if (userHistories.Count == 0)
                {
                    return NotFound("Không tìm thấy lịch sử để xóa.");
                }
                _movieContext.Histories.RemoveRange(userHistories);
                await _movieContext.SaveChangesAsync();
                return Ok();
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
                DateTime oneMonthAgo = DateTime.UtcNow.AddMonths(-1);
                var userHistories = await _movieContext.Histories.Where(h => h.UserId == userId && h.WatchedDate >= oneMonthAgo).ToListAsync();
                if (userHistories.Count == 0)
                {
                    return NotFound("Không tìm thấy lịch sử để xóa.");
                }
                _movieContext.Histories.RemoveRange(userHistories);
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
