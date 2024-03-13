
using System.ComponentModel.DataAnnotations;

namespace MovieAppApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Avatar { get; set; }
        public List<History>? Histories {  get; set; }
        public ICollection<WatchList>? WatchList { get; set; }
    }
}
