using MovieAppApi.Models;
using System.Collections;

namespace MovieAppApi.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Avatar { get; set; }
        public ArrayList? Histories { get; set; }
        public ArrayList? WatchLists { get; set; }
    }
}
