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
            var allHistories = await _movieContext.Histories
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.WatchedDate)
                .ToListAsync();
            return Ok(allHistories);
        }

        [HttpGet("{limit}/{userId}")]
        public async Task<IActionResult> GetLimitNewestHistory(int userId, int limit)
        {
            var latestHistories = await _movieContext.Histories
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.WatchedDate)
                .Take(limit)
                .ToListAsync();
            return Ok(latestHistories);
        }

        [HttpGet("{bottom}/{top}/{userId}")]
        public async Task<IActionResult> GetLimitNewestHistory(int userId, int bottom, int top)
        {
            var latestHistories = await _movieContext.Histories
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.WatchedDate)
                .Skip(bottom)
                .Take(top - bottom)
                .ToListAsync();
            return Ok(latestHistories);
        }

        [HttpPost("add")]
        public async  Task<IActionResult> AddNewHistory(HistoryDTO historyDTO)
        {
            var preHistory = new History();
            preHistory.UserId = historyDTO.UserId;
            preHistory.InformationMovie = historyDTO.InformationMovie;
            preHistory.WatchedDate = historyDTO.WatchedDate;
            preHistory.SecondsCount = historyDTO.SecondsCount;
            var newHistory = await _movieContext.Histories.AddAsync(preHistory);
            return Ok(newHistory);
        }

        [HttpPut("edit/{historyId}")]
        public async Task<IActionResult> UpdateHistory(int historyId, [FromBody] HistoryDTO historyDTO)
        {
            if (historyId != historyDTO.Id)
            {
                return BadRequest();
            }
            var preHistory = new History();
            preHistory.UserId = historyDTO.UserId;
            preHistory.InformationMovie = historyDTO.InformationMovie;
            preHistory.WatchedDate = historyDTO.WatchedDate;
            preHistory.SecondsCount = historyDTO.SecondsCount;
            _movieContext.Entry(preHistory).State = EntityState.Modified;

            try
            {
                await _movieContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_movieContext.Histories.Any(h => h.Id == historyId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("delete-all")]
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

        [HttpDelete("delete-one/{historyId}")]
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

        [HttpDelete("delete-last-hour")]
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
        [HttpDelete("delete-last-day")]
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
        [HttpDelete("delete-last-week")]
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
        [HttpDelete("delete-last-month")]
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
