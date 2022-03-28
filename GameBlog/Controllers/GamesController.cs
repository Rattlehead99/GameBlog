using GameBlog.Data;
using GameBlog.Data.Models;
using GameBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Index(string searchText, int pageIndex)
        {
            int pageSize = 6;


            var games = db.Games.OrderBy(g => g.Name).Skip(pageIndex*pageSize).Take(pageSize).AsQueryable();

            if (!String.IsNullOrEmpty(searchText))
            {
                games = games
                    .Where(s => s.Name.Contains(searchText) || s.Description.Contains(searchText));
            }

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

        //GET:
        public IActionResult Details(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var game = db.Games
                .Include(g => g.Ratings)
                .FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            var gameView = new GameViewModel
            {
                Description=game.Description,
                Genre=game.Genre,
                Id=game.Id,
                ImageUrl=game.ImageUrl,
                Name=game.Name,
                Ratings=game.Ratings
            };

            return View(gameView);
        }

        [HttpPost]
        public async Task<IActionResult> RateGame(RatingViewModel rating)
        {
            var user = await userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                return View(rating);
            }

            var gameData = db.Games.Any(g => g.Id == rating.GameId);

            if (!gameData)
            {
                return NotFound();
            }

            var currentRating = db.Ratings.FirstOrDefault(r => r.UserId == user.Id && r.GameId == rating.GameId);
            
            if (currentRating != null)
            {
                currentRating.RatingValue = rating.RatingValue;

                db.Ratings.Update(currentRating);
                db.SaveChanges();

                return RedirectToAction(nameof(Details), new { id = rating.GameId });
            }

            var ratingData = new Rating
            {
                GameId = rating.GameId,
                RatingValue = rating.RatingValue,
                UserId = user.Id
            };

            db.Ratings.Add(ratingData);
            db.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = rating.GameId });
        }
    }
}
