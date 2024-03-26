﻿namespace MovieAppApi.Models
{
    public class History
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int InformationMovieId { get; set; }
        public InformationMovie InformationMovie { get; set; } = null!;
        public DateTime WatchedDate { get; set; }
        public int SecondsCount { get; set; }
    }
}
