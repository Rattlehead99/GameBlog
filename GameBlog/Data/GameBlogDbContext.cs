using GameBlog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameBlog.Data
{
    public class GameBlogDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
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
               .HasForeignKey(r => r.GameId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(r => r.Ratings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(u => u.User)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserReputations>()
                .HasKey(ur => new { ur.UserId, ur.LikedUserId });

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserReputations)
                .WithOne(u => u.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

           /* modelBuilder.Entity<User>().HasData(
                user = new User
                {
                    FullName = "Kuche kotka",
                    Email = "ikovachev99@gmail.com",
                    Articles = new List<Article>(),
                    Password = "qaz12345",
                    Reputation = 1,
                    Ratings = new List<Rating>(),
                    Comments = new List<Comment>()
                });*/
          
            /*modelBuilder.Entity<Article>().HasData(
                new Article
                {
                    Title = "Call of Duty",
                    Content = "Who gives a damn for this one?",
                    ImageUrl = "https://images.unsplash.com/photo-1453728013993-6d66e9c9123a?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxzZWFyY2h8Mnx8dmlld3xlbnwwfHwwfHw%3D&w=1000&q=80",
                    User = user,
                    Comments = new List<Comment>()
                });*/

            base.OnModelCreating(modelBuilder);

            
        }

    }
}