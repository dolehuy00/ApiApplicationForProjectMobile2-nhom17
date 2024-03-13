using MovieAppApi.Models;
using System.Collections;

namespace MovieAppApi.DTO
{
    public class WatchListDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Title { get; set; }
        public ArrayList? WatchListDetails { get; set; }
    }
}
