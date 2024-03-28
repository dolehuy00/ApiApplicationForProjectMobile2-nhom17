namespace MovieAppApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; } = null!;
        public string? Password { get; set; }
        public string? Avatar { get; set; }
        public string? SubUserId { get; set; }
        public string? TagSocialNetwork { get; set; }
        public List<History>? Histories { get; set; }
        public ICollection<WatchList>? WatchLists { get; set; }
    }
}
