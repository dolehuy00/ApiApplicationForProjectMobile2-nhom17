using MovieAppApi.DTO;
using MovieAppApi.Models;

namespace MovieAppApi.Service
{
    public class BuildJSON
    {
        public dynamic UserCheckLogin(User user, string token)
        {
            return new
            {
                user.Id,
                token,
                user.Name,
                user.Email,
                user.Avatar,
            };
        }
        public dynamic WatchListAll(ICollection<WatchList> watchLists)
        {
            var watchListDTOs = watchLists
                .Select(source => new
                {
                    source.Id,
                    source.UserId,
                    source.Title,

                })
                .ToList();
            return watchListDTOs;
        }
        public dynamic WatchListGet(WatchList watchList)
        {
            return new
            {
                watchList.Id,
                watchList.UserId,
                watchList.Title
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
                watchListItem.Id,
                watchListItem.WatchListId,
                watchListItem.InformationMovie
            };
        }
    }
}
