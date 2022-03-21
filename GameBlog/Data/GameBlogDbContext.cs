using GameBlog.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameBlog.Data
{
    public class GameBlogDbContext : IdentityDbContext
    {
        public DbSet<Article> Articles { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<User> Users { get; set; }

        public GameBlogDbContext(DbContextOptions<GameBlogDbContext> options)
            : base(options)
        {
          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rating>()
                .HasKey(r => new { r.UserId, r.GameId });
            
            modelBuilder.Entity<Rating>()
               .HasOne(r => r.Game)
               .WithMany(r => r.Ratings)
               .HasForeignKey(r => r.GameId);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(r => r.Ratings)
                .HasForeignKey(r => r.UserId);

            base.OnModelCreating(modelBuilder);
        }

    }
}