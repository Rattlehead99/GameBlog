using GameBlog.Data.Models;
using GameBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlog.Test.TestConstants
{
    public static class TestData
    {
        public static readonly Guid ArticleId = Guid.NewGuid();
        public static readonly Guid GameId = Guid.NewGuid();
        public static readonly Guid UserId = Guid.NewGuid();

        public static ArticleViewModel ArticleView => new ArticleViewModel()
        {
            Content = "Bulshiser that should reach 30 symbols at the very leasfdhhgfcdsvhfsdgsdfgdfsgdsfgsdfgdfst",
            ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
            Title = "Some dumb title123123",
            Approved = false
        };

        public static ArticleViewModel ArticleViewWithId => new ArticleViewModel()
        {
            Id = ArticleId,
            Content = "Bulshiser that should reach 30 symbols at the very leasfdhhgfcdsvhfsdgsdfgdfsgdsfgsdfgdfst",
            ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
            Title = "Some dumb title123123",
            Approved = false
        };

        public static ArticleViewModel ArticleViewWithIdAndApproved => new ArticleViewModel()
        {
            Id = ArticleId,
            Content = "Bulshiser that should reach 30 symbols at the very leasfdhhgfcdsvhfsdgsdfgdfsgdsfgsdfgdfst",
            ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
            Title = "Some dumb title123123",
            Approved = true
        };

        public static GameViewModel GameView => new GameViewModel()
        {
            Description = "Some Description that is 30 symbols long maybe",
            Genre = "Action-Adventure",
            ImageUrl= "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
            Name = "Best Game Ever",
            Ratings = new List<Rating>
            {
                new Rating()
                {
                    GameId = GameId,
                    UserId = UserId
                }
            }
        };

        public static GameViewModel GameViewWithId => new GameViewModel()
        {
            Id = GameId,
            Description = "Some Description that is 30 symbols long maybe",
            Genre = "Action-Adventure",
            ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
            Name = "Best Game Ever",
            Ratings = new List<Rating>
            {
                new Rating()
                {
                    GameId = GameId,
                    UserId = UserId
                }
            }
        };

        public static GameViewModel GameViewWithOutRatings => new GameViewModel()
        {
            Id = GameId,
            Description = "Some Description that is 30 symbols long maybe",
            Genre = "Action-Adventure",
            ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
            Name = "Best Game Ever",
            Ratings = new List<Rating>()
        };

        public static RatingViewModel RatingView => new RatingViewModel()
        {
            GameId = GameId,
            RatingValue = 10
        };
        public static RatingViewModel RatingViewWithoutRatingValue => new RatingViewModel()
        {
            GameId = GameId
        };

        public static UserViewModel UserView => new UserViewModel()
        {
            Id = UserId,
            Articles = new List<Article>(),
            Email = "smth@gmail.com",
            Ratings = new List<Rating>
            {
                new Rating()
                {
                    GameId = GameId,
                    UserId = UserId
                }
            },
            Reputation = 1,
            UserName = "smth@gmail.com"
            
        };
    }
}
