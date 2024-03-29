﻿namespace GameBlog.Services
{
    using GameBlog.Data;
    using GameBlog.Data.Models;
    using GameBlog.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Claims;
    using static GameBlog.Data.DataConstants.Role;

    public class GamesService : IGamesService
    {
        private readonly GameBlogDbContext db;
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPaginationService paginationService;

        public GamesService(GameBlogDbContext db, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IPaginationService paginationService)
        {
            this.db = db;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.paginationService = paginationService;
        }


        public void CreateGame(GameViewModel game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
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
        }

        public void DeleteGame([FromForm] Guid id)
        {

            var game = db.Games.Find(id);

            if (game == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var rating = db.Ratings.Where(r => r.GameId == game.Id);

            if (rating == null)
            {
                throw new ArgumentNullException(nameof(rating));
            }

            db.Ratings.RemoveRange(rating);
            db.Games.Remove(game);
            db.SaveChanges();
        }

        public GameViewModel Details(Guid id)
        {
            var game = db.Games
               .Include(g => g.Ratings)
               .FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            var gameView = new GameViewModel
            {
                Description = game.Description,
                Genre = game.Genre,
                Id = game.Id,
                ImageUrl = game.ImageUrl,
                Name = game.Name,
                Ratings = game.Ratings
            };

            return gameView;
        }

        public void EditGame(GameViewModel game)
        {
            var gameData = db.Games.Find(game.Id);

            if (gameData == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            gameData.Description = game.Description;
            gameData.ImageUrl = game.ImageUrl;
            gameData.Name = game.Name;
            gameData.Genre = game.Genre;

            db.Games.Update(gameData);
            db.SaveChanges();
        }

        public AllGamesViewModel GetAllGames(int pageNumber, string searchText, string sortOrder)
        {
            var games = db.Games
                .OrderBy(g => g.Name)
                .AsQueryable();

            switch(sortOrder)
            {
                case "Alphabetical":
                    games = games.OrderBy(g => g.Name).AsQueryable();
                    break;
                case "Popularity":
                    games = games.OrderByDescending(g => g.Ratings.Count()).AsQueryable();
                    break;
                case "Rating":
                    games = games.OrderByDescending(g => Math.Round(g.Ratings.Average(r => r.RatingValue), 2));
                    break;
                default:
                    games = games.OrderBy(g => g.Name).AsQueryable();
                    break;
            }

            int newPageNumber = paginationService
                .PageCorrection(pageNumber, games);
            games = paginationService.Pagination(newPageNumber, games);

            if (!String.IsNullOrEmpty(searchText))
            {
                games = games
                    .Where(s => s.Name.Contains(searchText) || s.Description.Contains(searchText) || s.Genre.Contains(searchText));
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
                Games = gamesData,
                PageNumber = newPageNumber
            };

            return allGames;
        }

        public GameViewModel GetGameById(Guid id)
        {
            Game? game = db.Games.SingleOrDefault(x => x.Id == id);

            var gameView = new GameViewModel
            {
                Id = game.Id,
                Description = game.Description,
                Genre = game.Genre,
                ImageUrl = game.ImageUrl,
                Name = game.Name,
                Ratings = game.Ratings
            };

            return gameView;
        }

        public async Task RateGame(RatingViewModel rating)
        {
            ClaimsPrincipal? userContext = httpContextAccessor.HttpContext?.User;
            User? user = await userManager.GetUserAsync(userContext);

            bool gameData = db.Games.Any(g => g.Id == rating.GameId);

            if (!gameData)
            {
                throw new ArgumentNullException(nameof(gameData));
            }

            var currentRating = db.Ratings
                .FirstOrDefault(r => r.UserId == user.Id && r.GameId == rating.GameId);

            if (currentRating != null)
            {
                currentRating.RatingValue = rating.RatingValue;

                db.Ratings.Update(currentRating);
                db.SaveChanges();
                return;
            }
                var ratingData = new Rating
                {
                    GameId = rating.GameId,
                    RatingValue = rating.RatingValue,
                    UserId = user.Id
                };

                db.Ratings.Add(ratingData);
                db.SaveChanges();
        }
    }
}
