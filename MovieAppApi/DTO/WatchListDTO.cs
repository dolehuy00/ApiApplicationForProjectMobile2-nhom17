namespace MovieAppApi.DTO
{
    public class WatchListDTO
    {
        public WatchListDTO(int id, int userId, string title, int itemCount)
        {
            Id = id;
            UserId = userId;
            Title = title;
            ItemCount = itemCount;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
        public int ItemCount { get; set; }
    }
}
