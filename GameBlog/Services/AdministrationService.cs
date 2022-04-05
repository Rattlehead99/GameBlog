namespace GameBlog.Services
{
    using GameBlog.Data;
    using GameBlog.Data.Models;
    using GameBlog.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using static Data.DataConstants.Role;

    public class AdministrationService : IAdministrationService
    {
        private readonly GameBlogDbContext db;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;

        public AdministrationService(GameBlogDbContext db, UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public UserViewModel AdministratedUser(Guid id)
        {
            User? user = db.Users.SingleOrDefault(u => u.Id == id);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            List<Article> userArticles = db.Articles
               .Where(a => a.UserId == user.Id)
               .ToList();

            List<Guid> gameIds = db.Ratings
                .Where(x => x.UserId == user.Id)
                .Select(x => x.GameId)
                .Distinct()
                .ToList();

            List<Game> userGames = db.Games
                .Include(r => r.Ratings)
                .Where(g => gameIds.Contains(g.Id))
                .ToList();

            UserViewModel userViewModel = new UserViewModel
            {
                Email = user.Email,
                Articles = userArticles,
                Id = user.Id,
                Ratings = user.Ratings,
                Reputation = user.Reputation,
                UserName = user.UserName
            };

            return userViewModel;
        }

        public AllUsersViewModel AllUsers(string searchText)
        {
            var usersQuery = db.Users.AsQueryable();

            if (!String.IsNullOrEmpty(searchText))
            {
                usersQuery = usersQuery
                    .Where(uq => uq.UserName.Contains(searchText) || uq.Email.Contains(searchText));
            }

            var users = usersQuery.Select(u => new UserViewModel
            {
                UserName = u.UserName,
                Articles = u.Articles,
                Email = u.Email,
                Id = u.Id,
                Ratings = u.Ratings,
                Reputation = u.Reputation
            })
             .ToList();

            return new AllUsersViewModel
            {
                Users = users
            };
        }

        public async Task ChangeUserRole(Guid id)
        {
            User? user = db.Users.SingleOrDefault(u => u.Id == id);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IdentityUserRole<Guid>? userRoles = db.UserRoles.SingleOrDefault(ur => ur.UserId == user.Id);

            if (userRoles != null)
            {
                await userManager.RemoveFromRoleAsync(user, Administrator);
                return;
            }

            await userManager.AddToRoleAsync(user, Administrator);

            db.SaveChanges();

        }
    }
}
