using Microsoft.EntityFrameworkCore;
using MovieAppApi.Models;

namespace MovieAppApi.Data
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<WatchList> WatchLists { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<WatchListItem> WatchListItems { get; set; }
        public DbSet<ReviewVideo> ReviewVideos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<WatchList>().ToTable("WatchList");
            modelBuilder.Entity<History>().ToTable("History");
            modelBuilder.Entity<WatchListItem>().ToTable("WatchListItem");
            modelBuilder.Entity<ReviewVideo>().ToTable("ReviewVideo");
        }
    }
}
