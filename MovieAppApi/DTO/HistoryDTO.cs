using MovieAppApi.Models;

namespace MovieAppApi.DTO
{
    public class HistoryDTO
    {
        public HistoryDTO(int id, int userId, InformationMovieDTO informationMovie, DateTime watchedDate, int secondsCount)
        {
            Id = id;
            UserId = userId;
            InformationMovie = informationMovie;
            WatchedDate = watchedDate;
            SecondsCount = secondsCount;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime WatchedDate { get; set; }
        public int SecondsCount { get; set; }
        public InformationMovieDTO InformationMovie { get; set; }

    }
}
