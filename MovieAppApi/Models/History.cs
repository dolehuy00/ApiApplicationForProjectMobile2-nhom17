﻿using System.ComponentModel.DataAnnotations;

namespace MovieAppApi.Models
{
    public class History
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public User User { get; set; } = null!;
        public string? InformationMovie { get; set; }
        public DateTime? WatchedDate { get; set;}
        public int SecondsCount { get; set; }
    }
}
