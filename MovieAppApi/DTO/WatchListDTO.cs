namespace MovieAppApi.DTO
{
    public class WatchListDTO
    {
        public WatchListDTO(int id, int userId, string title)
        {
            Id = id;
            UserId = userId;
            Title = title;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
    }
}
