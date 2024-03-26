namespace MovieAppApi.Models
{
    public class WatchListItem
    {
        public int Id { get; set; }
        public int WatchListId { get; set; }
        public WatchList? WatchList { get; set; }
        public int InformationMovieId { get; set; }
        public InformationMovie InformationMovie { get; set; } = null!;
    }
}
