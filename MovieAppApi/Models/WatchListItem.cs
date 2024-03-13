namespace MovieAppApi.Models
{
    public class WatchListItem
    {
        public int Id { get; set; }
        public int WatchListId { get; set; }
        public WatchList? WatchList { get; set; }
        public string? InformationMovie { get; set; }
    }
}
