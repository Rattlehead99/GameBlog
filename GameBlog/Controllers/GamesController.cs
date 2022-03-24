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

        //GET:
        public IActionResult Edit(Guid id)
        {
            Game? game = db.Games.SingleOrDefault(x => x.Id == id);

            var gameView = new GameViewModel
            {
                Id = game.Id,
                Description = game.Description,
                Genre=game.Genre,
                ImageUrl = game.ImageUrl,
                Name=game.Name,
                Ratings=game.Ratings
            };

            return View(gameView);
        }

        [HttpPost]
        public IActionResult Edit(GameViewModel game)
        {
            if (!ModelState.IsValid)
            {
                return View(game);
            }

            var gameData = db.Games.Find(game.Id);

            if (gameData == null)
            {
                return NotFound();
            }

            gameData.Description = game.Description;
            gameData.ImageUrl = game.ImageUrl;
            gameData.Name = game.Name;
            gameData.Genre = game.Genre;

            db.Games.Update(gameData);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
           
        //GET:
        public IActionResult Delete(Guid? id)
        {
            var game = db.Games.Find(id);

            if (game == null)
            {
                return NotFound();
            }

            var gameModel = new GameViewModel
            {
                Description = game.Description,
                Genre = game.Genre,
                Id = game.Id,
                ImageUrl = game.ImageUrl,
                Name = game.Name,
                Ratings = game.Ratings
            };

            return View(gameModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteForm(Guid? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var game = db.Games.Find(id);

            if (game == null)
            {
                return NotFound();
            }

            db.Games.Remove(game);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
