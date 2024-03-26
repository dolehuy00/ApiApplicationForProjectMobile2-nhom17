using MovieAppApi.Models;

namespace MovieAppApi.DTO
{
    public class ReviewVideoDTO
    {
        public int Id { get; set; }
        public string MovieId { get; set; } = null!;
        public InformationMovieDTO InformationReviewVideo { get; set; } = null!;
    }
}
