using MovieAppApi.Models;

namespace MovieAppApi.Service
{
    public class BuildJSON
    {

        //
        // User
        //
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



        //
        // WatchList
        //
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



        //
        // WatchListItem
        //
        public dynamic WatchListItemAll(ICollection<WatchListItem> watchListItems)
        {
            var watchListItemDTOs = watchListItems
                .Select(source => new
                {
                    source.Id,
                    source.WatchListId,
                    InformationMovie = InformationMovieGet(source.InformationMovie)
                }).ToList();
            return watchListItemDTOs;
        }
        public dynamic WatchListItemGet(WatchListItem watchListItem)
        {
            return new
            {
                watchListItem.Id,
                watchListItem.WatchListId,
                InformationMovie = InformationMovieGet(watchListItem.InformationMovie)
            };
        }



        //
        // History
        //
        public dynamic HistoryItemAll(ICollection<History> histories)
        {
            var historiesDTOs = histories
                .Select(source => new
                {
                    source.Id,
                    source.UserId,
                    source.WatchedDate,
                    source.SecondsCount,
                    InformationMovie = InformationMovieGet(source.InformationMovie)
                }).ToList();
            return historiesDTOs;
        }

        public dynamic HistoryGet(History history)
        {
            return new
            {
                history.Id,
                history.UserId,
                history.WatchedDate,
                history.SecondsCount,
                InformationMovie = InformationMovieGet(history.InformationMovie)
            };
        }


        //
        // Information Movie
        //
        public dynamic InformationMovieGet(InformationMovie informationMovie)
        {
            return new
            {
                informationMovie.Id,
                informationMovie.MovieId,
                informationMovie.Title,
                informationMovie.Tag,
                informationMovie.ImageLink,
                informationMovie.Durations
            };
        }



        //
        // Review Video
        //
        public dynamic ReviewVideoAll(ICollection<ReviewVideo> reviewVideos)
        {
            var reviewVideosDTOs = reviewVideos
                .Select(source => new
                {
                    source.Id,
                    source.MovieId,
                    InformationReviewVideo = InformationMovieGet(source.InformationReviewVideo)
                }).ToList();
            return reviewVideosDTOs;
        }

        public dynamic ReviewVideoGet(ReviewVideo reviewVideo)
        {
            return new
            {
                reviewVideo.Id,
                reviewVideo.MovieId,
                InformationReviewVideo = InformationMovieGet(reviewVideo.InformationReviewVideo)
            };
        }
    }
}
