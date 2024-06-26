﻿using MovieAppApi.Models;

namespace MovieAppApi.DTO
{
    public class WatchListItemDTO
    {
        public int Id { get; set; }
        public int WatchListId { get; set; }
        public InformationMovieDTO InformationMovie { get; set; } = null!;
    }
}
