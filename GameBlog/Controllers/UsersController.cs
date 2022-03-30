using GameBlog.Data;
using GameBlog.Data.Models;
using GameBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GameBlog.Controllers
{
    public class UsersController : Controller
    {
        private readonly GameBlogDbContext db;
        private readonly UserManager<User> userManager;

        public UsersController(GameBlogDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        //GET
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);

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

            return View(userViewModel);
        }

        public IActionResult All(string searchText)
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

            return View(new AllUsersViewModel
            {
                Users = users
            }) ;
        }
    }
}
