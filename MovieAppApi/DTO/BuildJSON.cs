using MovieAppApi.Models;

namespace MovieAppApi.DTO
{
    public class BuildJSON
    {
        public dynamic UserCheckLogin(User user)
        {
            return new
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Avatar = user.Avatar,
            };
        }
    }
}
