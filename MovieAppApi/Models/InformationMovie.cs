namespace MovieAppApi.Models
{
    public class InformationMovie
    {
        public int Id { get; set; }
        public string MovieId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string ImageLink { get; set; } = null!;
        public string Tag { get; set; } = null!;
        public History? History { get; set; }
        public ReviewVideo? ReviewVideo { get; set; }
        public WatchListItem? WatchListItem { get; set; }
    }
}
