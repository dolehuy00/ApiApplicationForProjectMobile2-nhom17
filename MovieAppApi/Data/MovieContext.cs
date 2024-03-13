using Microsoft.EntityFrameworkCore;
using MovieAppApi.Models;
using System.Reflection.Metadata;

namespace MovieAppApi.Data
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {  
        }

        public DbSet<User> Users { get; set; }
        public DbSet<WatchList> WatchList { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<WatchListDetail> ListDetail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<WatchList>().ToTable("WatchList");
            modelBuilder.Entity<History>().ToTable("History");
            modelBuilder.Entity<WatchListDetail>().ToTable("WatchListDetail");
            modelBuilder.Entity<User>()
                            .HasMany(u => u.Histories)
                            .WithOne(h => h.User)
                            .HasForeignKey(h => h.UserId)
                            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>()
                            .HasMany(e => e.WatchList)
                            .WithOne(e => e.User)
                            .HasForeignKey(e => e.UserId)
                            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<WatchList>()
                            .HasMany(e => e.WatchListDetail)
                            .WithOne(e => e.WatchList)
                            .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
