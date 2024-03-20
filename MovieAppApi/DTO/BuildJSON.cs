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
            var watchListDTOs = watchLists
                .Select(source => new
                {
                    Id = source.Id,
                    UserId = source.UserId,
                    Title = source.Title,
                
                })
                .ToList();
            return watchListDTOs;
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

        public dynamic WatchListItemAll(ICollection<WatchListItem> watchListItems)
        {
            var watchListItemDTOs = watchListItems
                .Select(source => new WatchListItemDTO
                {
                    Id = source.Id,
                    WatchListId = source.WatchListId,
                    InformationMovie = source.InformationMovie
                }).ToList();
            return watchListItemDTOs;
        }
        public dynamic WatchListItemGet(WatchListItem watchListItem)
        {
            return new
            {
                Id = watchListItem.Id,
                WatchListId = watchListItem.WatchListId,
                InformationMovie = watchListItem.InformationMovie
            };
        }
    }
}
