using GameBlog.Data;
using GameBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameBlog.Controllers
{
    public class GamesController : Controller
    {
        private readonly GameBlogDbContext db;

        public GamesController(GameBlogDbContext db)
        {
            this.db = db;
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
                Ratings = g.Ratings
            })
            .ToList();

            var allGames = new AllGamesViewModel
            {
                Search = "null",
                Games = gamesData
            };

            return View(allGames);
        }
    }
}
