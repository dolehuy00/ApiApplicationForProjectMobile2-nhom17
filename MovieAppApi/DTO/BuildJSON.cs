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
        public dynamic WatchListAll(ICollection<WatchList> watchLists) 
        {
            var watchListDTO = watchLists
                .Select(source => new
                {
                    Id = source.Id,
                    UserId = source.UserId,
                    Title = source.Title,
                
                })
                .ToList();
            return watchListDTO;
        }
        public dynamic WatchListGet(WatchList watchList)
        {
            return new
            {
                Id = watchList.Id,
                UserId = watchList.UserId,
                Title = watchList.Title
            };
        }
    }
}
