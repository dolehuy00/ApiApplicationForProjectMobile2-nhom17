namespace MovieAppApi.Models
{
    public class ReviewVideo
    {
        public int Id { get; set; }
        public string MovieId { get; set; } = null!;
        public int InformationReviewVideoId { get; set; }
        public InformationMovie InformationReviewVideo { get; set; } = null!;
    }
}
