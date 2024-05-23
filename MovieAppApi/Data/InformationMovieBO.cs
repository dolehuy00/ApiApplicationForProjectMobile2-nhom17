using Microsoft.EntityFrameworkCore;
using MovieAppApi.Models;

namespace MovieAppApi.Data
{
    public class InformationMovieBO
    {
        private MovieContext _movieContext;

        public InformationMovieBO(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }


        public async Task<InformationMovie> AddOrUpdateInformationMovie(InformationMovie informationMovie)
        {
            try
            {
                var oldInfomationMovie = await _movieContext.InformationMovies
                    .Where(i => i.MovieId == informationMovie.MovieId && i.Tag == informationMovie.Tag)
                    .FirstOrDefaultAsync();
                if (oldInfomationMovie == null)
                {
                    _movieContext.Add(informationMovie);
                    await _movieContext.SaveChangesAsync();
                    return informationMovie;
                }
                else
                {
                    oldInfomationMovie.Title = informationMovie.Title;
                    oldInfomationMovie.ImageLink = informationMovie.ImageLink;
                    oldInfomationMovie.Durations = informationMovie.Durations;
                    await _movieContext.SaveChangesAsync();
                    return oldInfomationMovie;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
