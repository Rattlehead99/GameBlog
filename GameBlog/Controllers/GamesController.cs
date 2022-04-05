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
            var gameView = gamesService.GetGameById(id);

            return View(gameView);
        }

        [HttpPost]
        [Authorize(Roles = Administrator)]
        public IActionResult Edit(GameViewModel game)
        {

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
            var gameModel = gamesService.GetGameById(id);

            return View(gameModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = Administrator)]
        public IActionResult DeleteForm(Guid id)
        {
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
            if (!ModelState.IsValid)
            {
                return View(rating);
            }

            await gamesService.RateGame(rating);

            return RedirectToAction(nameof(Details), new { id = rating.GameId });
        }
    }
}
