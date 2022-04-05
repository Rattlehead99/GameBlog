using GameBlog.Data;
using GameBlog.Data.Models;
using GameBlog.Models;
using GameBlog.Services;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUsersService usersService;

        public UsersController(GameBlogDbContext db, UserManager<User> userManager, IUsersService usersService)
        {
            this.db = db;
            this.userManager = userManager;
            this.usersService = usersService;
        }

        //GET
        [Authorize]
        public async Task<IActionResult> Index()
        {
            //var user = await userManager.GetUserAsync(User);

            //List<Article> userArticles = db.Articles
            //    .Where(a => a.UserId == user.Id)
            //    .ToList();

            //List<Guid> gameIds = db.Ratings
            //    .Where(x => x.UserId == user.Id)
            //    .Select(x => x.GameId)
            //    .Distinct()
            //    .ToList();

            //List<Game> userGames = db.Games
            //    .Include(r => r.Ratings)
            //    .Where(g => gameIds.Contains(g.Id))
            //    .ToList();


            //UserViewModel userViewModel = new UserViewModel
            //{
            //    Email = user.Email,
            //    Articles = userArticles,
            //    Id = user.Id,
            //    Ratings = user.Ratings,
            //    Reputation = user.Reputation,
            //    UserName = user.UserName
            //};

            var userViewModel = await usersService.GetUserProfile();

            return View(userViewModel);
        }

        [Authorize]
        public IActionResult All(int pageNumber=1, string searchText="")
        {
            //int pageSize = 6;
            //double pageCount = Math.Ceiling(db.Articles.Count() / (double)pageSize);

            //if (pageNumber < 1)
            //{
            //    return RedirectToAction("Index", new { pageNumber = 1 });
            //}
            //if (pageNumber > pageCount)
            //{
            //    return RedirectToAction("Index", new { pageNumber = pageCount });
            //}

            //var usersQuery = db.Users
            //    .Skip((pageNumber - 1) * pageSize)
            //    .Take(pageSize)
            //    .AsQueryable();

            //if (!String.IsNullOrEmpty(searchText))
            //{
            //    usersQuery = usersQuery
            //        .Where(uq => uq.UserName.Contains(searchText) || uq.Email.Contains(searchText));
            //}

            //var users = usersQuery.Select(u => new UserViewModel
            //{
            //    UserName = u.UserName,
            //    Articles = u.Articles,
            //    Email = u.Email,
            //    Id = u.Id,
            //    Ratings = u.Ratings,
            //    Reputation = u.Reputation
            //})
            // .ToList();

            var allUsers = usersService.GetAllUsers(pageNumber, searchText);

            return View(allUsers);
        }

        [Authorize]
        public IActionResult Profile(Guid id)
        {

            //User? user = db.Users.SingleOrDefault(u => u.Id == id);

            //if (user == null)
            //{
            //    return BadRequest();
            //}

            //List<Article> userArticles = db.Articles
            //   .Where(a => a.UserId == user.Id)
            //   .ToList();

            //List<Guid> gameIds = db.Ratings
            //    .Where(x => x.UserId == user.Id)
            //    .Select(x => x.GameId)
            //    .Distinct()
            //    .ToList();

            //List<Game> userGames = db.Games
            //    .Include(r => r.Ratings)
            //    .Where(g => gameIds.Contains(g.Id))
            //    .ToList();

            //UserViewModel userViewModel = new UserViewModel
            //{
            //    Email = user.Email,
            //    Articles = userArticles,
            //    Id = user.Id,
            //    Ratings = user.Ratings,
            //    Reputation = user.Reputation,
            //    UserName = user.UserName
            //};

            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var userViewModel = usersService.Profile(id);

            return View(userViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Rate(Guid id)
        {
            //User? loggedUser = await userManager.GetUserAsync(User);

            //User? user = db.Users
            //    .Include(ur => ur.UserReputations)
            //    .SingleOrDefault(x => x.Id == id);

            //if (user == null)
            //{
            //    return NotFound();
            //}

            //if (!user.UserReputations.Any(u => u.LikedUserId == loggedUser.Id))
            //{
            //    user.Reputation++;
            //    user.UserReputations.Add(new UserReputations { UserId = user.Id, User = user, LikedUserId = loggedUser.Id, LikedUser = loggedUser });
            //    db.SaveChanges();
            //}

            //if (!user.ReputationLikes.Contains(loggedUser.Id.ToString()))
            //{
            //    user.Reputation++;
            //    user.ReputationLikes.Add(loggedUser.Id.ToString());
            //    db.SaveChanges();

            //    db.Users.Update(user);
            //    db.SaveChanges();
            //}

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await usersService.Rate(id);

            return RedirectToAction("All");
        }
    }
}
