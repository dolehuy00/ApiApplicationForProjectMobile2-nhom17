using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppApi.Data;
using MovieAppApi.DTO;
using MovieAppApi.Models;
using MovieAppApi.Service;

namespace MovieAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewVideoController : ControllerBase
    {
        private MovieContext _movieContext;
        private BuildJSON buildJSON;
        public ReviewVideoController(MovieContext movieContext)
        {
            _movieContext = movieContext;
            this.buildJSON = new BuildJSON();
        }

        [HttpGet("all/{informationMovie}")]
        public async Task<IActionResult> GetAll(string informationMovie)
        {
            try
            {
                var all = await _movieContext.ReviewVideos
                    .Where(r => r.InformationMovie == informationMovie)
                    .ToListAsync();
                return Ok(all);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNew([FromBody] ReviewVideoDTO reviewVideoDTO)
        {
            try
            {
                var oldWatchListItem = await _movieContext.ReviewVideos
                    .Where(w => w.InformationReviewVideo == reviewVideoDTO.InformationReviewVideo)
                    .FirstOrDefaultAsync();
                if (oldWatchListItem != null)
                {
                    return BadRequest("Item is exist in review list");
                }
                var newItem = new ReviewVideo();
                newItem.InformationMovie = reviewVideoDTO.InformationMovie;
                newItem.InformationReviewVideo = reviewVideoDTO.InformationReviewVideo;
                await _movieContext.ReviewVideos.AddAsync(newItem);
                await _movieContext.SaveChangesAsync();
                return Ok(newItem);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("delete-one/{reviewVideoId}")]
        public async Task<IActionResult> DeleteAWatchListItem(int reviewVideoId)
        {
            try
            {
                var watchListItemToDelete = await _movieContext.ReviewVideos.Where(w => w.Id == reviewVideoId).FirstOrDefaultAsync();
                if (watchListItemToDelete == null)
                {
                    return NotFound("Không tìm thấy mục để xóa.");
                }
                _movieContext.ReviewVideos.Remove(watchListItemToDelete);
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
