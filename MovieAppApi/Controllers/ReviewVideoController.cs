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
        private InformationMovieBO _informationMovieBO;

        public ReviewVideoController(MovieContext movieContext)
        {
            _movieContext = movieContext;
            buildJSON = new BuildJSON();
            _informationMovieBO = new InformationMovieBO(movieContext);
        }

        [HttpGet("all/{movieId}")]
        public async Task<IActionResult> GetAll(string movieId)
        {
            try
            {
                var all = await _movieContext.ReviewVideos
                    .Where(r => r.MovieId == movieId)
                    .Include(r => r.InformationReviewVideo)
                    .ToListAsync();
                return Ok(buildJSON.ReviewVideoAll(all));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{limit}/{movieId}")]
        public async Task<IActionResult> GetLimit(string movieId, int limit)
        {
            try
            {
                var limitReviewVideos = await _movieContext.ReviewVideos
                    .Where(r => r.MovieId == movieId)
                    .Take(limit)
                    .Include(r => r.InformationReviewVideo)
                    .ToListAsync();
                return Ok(buildJSON.ReviewVideoAll(limitReviewVideos));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{skip}/{take}/{movieId}")]
        public async Task<IActionResult> GetLimitSkipTake(string movieId, int skip, int take)
        {
            try
            {
                var limitReviewVideos = await _movieContext.ReviewVideos
                    .Where(r => r.MovieId == movieId)
                    .Skip(skip)
                    .Take(take)
                    .Include(r => r.InformationReviewVideo)
                    .ToListAsync();
                return Ok(buildJSON.ReviewVideoAll(limitReviewVideos));
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
                var oldReviewVideo = await _movieContext.ReviewVideos
                    .Where(w => w.MovieId == reviewVideoDTO.MovieId
                        && w.InformationReviewVideo.MovieId == reviewVideoDTO.InformationReviewVideo.MovieId
                        && w.InformationReviewVideo.Tag == reviewVideoDTO.InformationReviewVideo.Tag)
                    .FirstOrDefaultAsync();
                if (oldReviewVideo == null)
                {
                    InformationMovie newInformationReviewVideo = new InformationMovie();
                    newInformationReviewVideo.Title = reviewVideoDTO.InformationReviewVideo.Title;
                    newInformationReviewVideo.MovieId = reviewVideoDTO.InformationReviewVideo.MovieId;
                    newInformationReviewVideo.ImageLink = reviewVideoDTO.InformationReviewVideo.ImageLink;
                    newInformationReviewVideo.Tag = reviewVideoDTO.InformationReviewVideo.Tag;

                    var newItem = new ReviewVideo();
                    newItem.MovieId = reviewVideoDTO.MovieId;
                    newItem.InformationReviewVideo = await _informationMovieBO.AddOrUpdateInformationMovie(newInformationReviewVideo);
                    await _movieContext.ReviewVideos.AddAsync(newItem);
                    await _movieContext.SaveChangesAsync();
                    return Ok(buildJSON.ReviewVideoGet(newItem));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("delete-one/{reviewVideoId}")]
        public async Task<IActionResult> DeleteOne(int reviewVideoId)
        {
            try
            {
                var itemToDelete = await _movieContext.ReviewVideos.Where(w => w.Id == reviewVideoId).FirstOrDefaultAsync();
                if (itemToDelete == null)
                {
                    return NotFound("Không tìm thấy mục để xóa.");
                }
                _movieContext.ReviewVideos.Remove(itemToDelete);
                await _movieContext.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }

        [HttpDelete("delete-many")]
        public async Task<IActionResult> DeleteMany([FromBody] int[] reviewVideoIds)
        {
            if (reviewVideoIds == null || reviewVideoIds.Length == 0)
            {
                return BadRequest("Danh sách id trống.");
            }
            try
            {
                var items = await _movieContext.ReviewVideos
                    .Where(w => reviewVideoIds.Contains(w.Id))
                    .ToListAsync();
                if (items.Count == 0)
                {
                    return NotFound("Không tìm thấy danh sách để xóa.");
                }
                _movieContext.ReviewVideos.RemoveRange(items);
                await _movieContext.SaveChangesAsync();
                return Ok(new { success = true });

            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }
    }
}
