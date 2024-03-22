namespace MovieAppApi.Models
{
    public class WatchList
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string Title { get; set; } = null!;
        public ICollection<WatchListItem>? WatchListDetails { get; set; }
    }
}
