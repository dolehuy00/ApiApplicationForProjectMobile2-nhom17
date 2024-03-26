namespace MovieAppApi.Models
{
    public class InformationMovieDTO
    {
        public int Id { get; set; }
        public string MovieId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string ImageLink { get; set; } = null!;
        public string Tag { get; set; } = null!;
    }
}
