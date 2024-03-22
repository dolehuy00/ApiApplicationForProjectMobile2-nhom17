namespace MovieAppApi.Models
{
    public class History
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? InformationMovie { get; set; }
        public DateTime? WatchedDate { get; set; }
        public int SecondsCount { get; set; }
    }
}
