﻿
namespace MovieAppApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Avatar { get; set; }
        public ICollection<History>? Histories { get; set; }
        public ICollection<WatchList>? WatchList { get; set; }
    }
}