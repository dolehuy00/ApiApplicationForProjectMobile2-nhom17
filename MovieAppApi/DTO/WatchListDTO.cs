namespace MovieAppApi.DTO
{
    public class WatchListDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
        public int ItemCount { get; set; }
        public WatchListItemDTO? item { get; set; }
    }
}
