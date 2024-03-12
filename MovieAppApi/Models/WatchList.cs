namespace MovieAppApi.Models
{
    public class WatchList
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Title { get; set; }
        public ICollection<WatchListDetail>? WatchListDetail { get; set; }
    }
}
