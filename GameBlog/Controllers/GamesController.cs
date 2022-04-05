using GameBlog.Data;
using GameBlog.Data.Models;
using GameBlog.Models;
using GameBlog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static GameBlog.Data.DataConstants.Role;

namespace GameBlog.Controllers
{
    public class GamesController : Controller
    {
        private readonly GameBlogDbContext db;
        private readonly UserManager<User> userManager;
        private readonly IGamesService gamesService;

        public GamesController(GameBlogDbContext db, UserManager<User> userManager, IGamesService gamesService)
        {
            this.db = db;
            this.userManager = userManager;
            this.gamesService = gamesService;
        }

        [AllowAnonymous]
        public IActionResult Index(int pageNumber, string searchText)
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

            //var games = db.Games.OrderBy(g => g.Name)
            //    .Skip((pageNumber - 1)*pageSize)
            //    .Take(pageSize)
            //    .AsQueryable();

            //if (!String.IsNullOrEmpty(searchText))
            //{
            //    games = games
            //        .Where(s => s.Name.Contains(searchText) || s.Description.Contains(searchText));
            //}

            //var gamesData = games.Select(g => new GameViewModel
            //{
            //    Description = g.Description,
            //    Genre = g.Genre,
            //    Id = g.Id,
            //    Name = g.Name,
            //    Ratings = g.Ratings,
            //    ImageUrl = g.ImageUrl
            //})
            //.ToList();

            //var allGames = new AllGamesViewModel
            //{
            //    Games = gamesData,
            //    PageNumber = pageNumber
            //};

            var allGames = gamesService.GetAllGames(pageNumber, searchText);

            return View(allGames);
        }

        //GET:
        [Authorize(Roles = Administrator)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Administrator)]
        public IActionResult Create(GameViewModel game)
        {
            //if (game == null)
            //{
            //    return NotFound();
            //}

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}

            //var gameData = new Game
            //{
            //    Name = game.Name,
            //    Description = game.Description,
            //    Genre = game.Genre,
            //    Id = game.Id,
            //    ImageUrl = game.ImageUrl,
            //    Ratings = game.Ratings,
                
            //};

            //db.Games.Add(gameData);
            //db.SaveChanges();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            gamesService.CreateGame(game);

            return RedirectToAction("Index");
        }

        //GET:
        [Authorize(Roles = Administrator)]
        public IActionResult Edit(Guid id)
        {
            //Game? game = db.Games.SingleOrDefault(x => x.Id == id);

            //var gameView = new GameViewModel
            //{
            //    Id = game.Id,
            //    Description = game.Description,
            //    Genre=game.Genre,
            //    ImageUrl = game.ImageUrl,
            //    Name=game.Name,
            //    Ratings=game.Ratings
            //};

            var gameView = gamesService.GetGameById(id);

            return View(gameView);
        }

        [HttpPost]
        [Authorize(Roles = Administrator)]
        public IActionResult Edit(GameViewModel game)
        {

            //var gameData = db.Games.Find(game.Id);

            //if (gameData == null)
            //{
            //    return NotFound();
            //}

            //gameData.Description = game.Description;
            //gameData.ImageUrl = game.ImageUrl;
            //gameData.Name = game.Name;
            //gameData.Genre = game.Genre;

            //db.Games.Update(gameData);
            //db.SaveChanges();

            if (!ModelState.IsValid)
            {
                return View(game);
            }

            gamesService.EditGame(game);

            return RedirectToAction("Index");
        }
           
        //GET:
        [Authorize]
        [Authorize(Roles = Administrator)]
        public IActionResult Delete(Guid id)
        {
            //var game = db.Games.Find(id);

            //if (game == null)
            //{
            //    return NotFound();
            //}

            //var gameModel = new GameViewModel
            //{
            //    Description = game.Description,
            //    Genre = game.Genre,
            //    Id = game.Id,
            //    ImageUrl = game.ImageUrl,
            //    Name = game.Name,
            //    Ratings = game.Ratings
            //};

            var gameModel = gamesService.GetGameById(id);

            return View(gameModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = Administrator)]
        public IActionResult DeleteForm(Guid id)
        {

            //var game = db.Games.Find(id);

            //if (game == null)
            //{
            //    return NotFound();
            //}

            //db.Games.Remove(game);
            //db.SaveChanges();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            gamesService.DeleteGame(id);

            return RedirectToAction("Index");
        }

        //GET:
        [AllowAnonymous]
        public IActionResult Details(Guid id)
        {
            //var game = db.Games
            //    .Include(g => g.Ratings)
            //    .FirstOrDefault(g => g.Id == id);

            //if (game == null)
            //{
            //    return NotFound();
            //}

            //var gameView = new GameViewModel
            //{
            //    Description=game.Description,
            //    Genre=game.Genre,
            //    Id=game.Id,
            //    ImageUrl=game.ImageUrl,
            //    Name=game.Name,
            //    Ratings=game.Ratings
            //};

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var gameView = gamesService.Details(id);

            return View(gameView);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RateGame(RatingViewModel rating)
        {
            //var user = await userManager.GetUserAsync(User);

            //var gameData = db.Games.Any(g => g.Id == rating.GameId);

            //if (!gameData)
            //{
            //    return NotFound();
            //}

            //var currentRating = db.Ratings
            //    .FirstOrDefault(r => r.UserId == user.Id && r.GameId == rating.GameId);
            
            //if (currentRating != null)
            //{
            //    currentRating.RatingValue = rating.RatingValue;

            //    db.Ratings.Update(currentRating);
            //    db.SaveChanges();

            //    return RedirectToAction(nameof(Details), new { id = rating.GameId });
            //}

            //var ratingData = new Rating
            //{
            //    GameId = rating.GameId,
            //    RatingValue = rating.RatingValue,
            //    UserId = user.Id
            //};
            
            //db.Ratings.Add(ratingData);
            //db.SaveChanges();

            if (!ModelState.IsValid)
            {
                return View(rating);
            }

            await gamesService.RateGame(rating);

            return RedirectToAction(nameof(Details), new { id = rating.GameId });
        }
    }
}
