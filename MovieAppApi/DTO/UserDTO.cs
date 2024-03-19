using MovieAppApi.Models;
using System.Collections;

namespace MovieAppApi.DTO
{
    public class UserDTO
    {
        public UserDTO(int id, string name, string password, string passwordConfirm, string email, string? avatar)
        {
            Id = id;
            Name = name;
            Password = password;
            PasswordConfirm = passwordConfirm;
            Email = email;
            Avatar = avatar;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string Email { get; set; }
        public string? Avatar { get; set; }
    }
}
