using GameBlog.Data;
using GameBlog.Data.Models;
using GameBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameBlog.Controllers
{
    public class GamesController : Controller
    {
        private readonly GameBlogDbContext db;
        private readonly UserManager<User> userManager;

        public GamesController(GameBlogDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var games = db.Games.AsQueryable();

            var gamesData = games.Select(g => new GameViewModel
            {
                Description = g.Description,
                Genre = g.Genre,
                Id = g.Id,
                Name = g.Name,
                Ratings = g.Ratings,
                ImageUrl = g.ImageUrl
            })
            .ToList();

            var allGames = new AllGamesViewModel
            {
                Search = "null",
                Games = gamesData
            };

            return View(allGames);
        }

        //GET:
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(GameViewModel game)
        {
            if (game == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var gameData = new Game
            {
                Name = game.Name,
                Description = game.Description,
                Genre = game.Genre,
                Id = game.Id,
                ImageUrl = game.ImageUrl,
                Ratings = game.Ratings,
                
            };

            db.Games.Add(gameData);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
           
    }
}
