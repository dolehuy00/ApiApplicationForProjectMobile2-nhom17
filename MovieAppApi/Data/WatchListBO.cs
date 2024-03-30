using Microsoft.EntityFrameworkCore;

namespace MovieAppApi.Data
{
    public class WatchListBO
    {
        private MovieContext _movieContext;

        public WatchListBO(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }


        public async Task PlustOneItemCount(int watchListId)
        {
            try
            {
                var watchList = await _movieContext.WatchLists
                    .Where(i => i.Id == watchListId)
                    .FirstOrDefaultAsync();
                if (watchList != null)
                {
                    watchList.ItemCount += 1;
                    await _movieContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task MinusItemCount(int watchListId, int minus)
        {
            try
            {
                var watchList = await _movieContext.WatchLists
                    .Where(i => i.Id == watchListId)
                    .FirstOrDefaultAsync();
                if (watchList != null)
                {
                    watchList.ItemCount -= minus;
                    await _movieContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
