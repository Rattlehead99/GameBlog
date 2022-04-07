using Castle.Core.Logging;
using Microsoft.AspNetCore.Authentication;


namespace GameBlog.Test.Services
{
    using Microsoft.Extensions.Options;
    using System.Security.Claims;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using Moq;
    using Microsoft.EntityFrameworkCore.InMemory;
    using GameBlog.Services;
    using GameBlog.Test.Mock;
    using GameBlog.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using GameBlog.Models;
    using GameBlog.Data;
    using System.Threading;
    using System.Security.Principal;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Mvc.Testing;
    using System.Net.Http.Json;
    using Microsoft.Extensions.DependencyInjection;
    using System.Net.Http.Headers;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.EntityFrameworkCore;
    using System.Text.Json;
    using System.Net.Http;
    using GameBlog.Test.Mock;
    using GameBlog.Test.TestConstants;

    public class GamesServiceTest : IClassFixture<CustomWebApplicationFactory>
    {
        private IGamesService gamesService;
        private readonly DependencyScope scope;

        public GamesServiceTest(CustomWebApplicationFactory factory)
        {
            scope = factory.InitDb();
            gamesService = scope.ResolveService<IGamesService>();
        }

        [Fact]
        public void CreateGame_Should_Add_Game_To_DB()
        {
            //Arrange
            GameViewModel gameView = TestData.GameView;

            //Act
            gamesService.CreateGame(gameView);
            var gamesCountAfterCreation = scope.Db.Games.Count();

            //Assert
            Assert.Equal(2, gamesCountAfterCreation);
        }

        [Fact]
        public void CreateGame_Should_Throw_If_GameView_Is_Null()
        {
            //Arrange
            GameViewModel gameView = null;

            //Act
            Action? action = () => gamesService.CreateGame(gameView);


            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void DeleteGame_Should_Remove_Game_From_DB()
        {
            //Arrange
            GameViewModel gameView = TestData.GameViewWithId;

            //Act
            var games = scope.ResolveService<GameBlogDbContext>().Games;
            gamesService.CreateGame(gameView);
            var initialGamesCount = games.Count();

            gamesService.DeleteGame(gameView.Id);

            var afterDeleteGamesCount = games.Count();

            //Assert
            Assert.NotEqual(afterDeleteGamesCount, initialGamesCount);
        }

        [Fact]
        public void DeleteGame_Should_Throw_When_Game_Is_Null()
        {
            //Arrange
            GameViewModel gameView = TestData.GameViewWithId;

            //Act
            Action? action = () => gamesService.DeleteGame(gameView.Id);

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void DeleteGame_Should_Throw_When_Rating_Is_Null()
        {
            //Arrange
            GameViewModel gameView = TestData.GameViewWithOutRatings;

            //Act
            Action? action = () => gamesService.DeleteGame(gameView.Id);

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void Details_Should_Return_GameViewModel()
        {
            //Arrange
            var game = scope.Db.Games.First();

            //Act
            var gameView = gamesService.Details(game.Id);

            //Assert
            Assert.IsType<GameViewModel>(gameView);
        }

        [Fact]
        public void Details_Should_Throw_If_Game_Is_Null()
        {
            //Arrange

            //Act
            Action? action = () => gamesService.Details(TestData.GameId);

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void EditGame_Should_Change_Game_Properties()
        {
            //Arrange
            var gameView = TestData.GameViewWithId;
            gamesService.CreateGame(gameView);

            var initialName = gameView.Name;
            var initialDescription = gameView.Description;

            var newName = "Something different";
            var newDescription = "More stuff to fill 30 sharacters with";

            gameView.Name = newName;
            gameView.Description = newDescription;

            //Act
            gamesService.EditGame(gameView);

            //Assert
            Assert.NotEqual(initialName, gameView.Name);
            Assert.NotEqual(initialDescription, gameView.Description);

            Assert.Equal(newName, gameView.Name);
            Assert.Equal(newDescription, gameView.Description);
        }

        [Fact]
        public void EditGame_Should_Throw_If_Game_Is_Null()
        {
            //Arrange
            var gameView = TestData.GameView;

            //Act
            var games = scope.ResolveService<GameBlogDbContext>().Games;
            Action? action = () => gamesService.EditGame(gameView);

            //Assert
            Assert.Throws<ArgumentNullException>(action);

        }

        [Fact]
        public void GetAllGames_Should_Return_AllGamesViewModel()
        {
            //Arrange
            int pageNumber = 1;
            string searchText = "";

            //Act
            var result = gamesService.GetAllGames(pageNumber, searchText);

            //Assert
            Assert.IsType<AllGamesViewModel>(result);
        }

        [Fact]
        public void GetGameById_Should_Return_GameViewModel()
        {
            //Arrange
            var gameView = TestData.GameView;

            //Act
            gamesService.CreateGame(gameView);
            var result = gamesService.GetGameById(gameView.Id);

            //Assert
            Assert.IsType<GameViewModel>(result);
        }

        //[Fact]
        //public void RateGame_Should_Add_Rating()
        //{
        //    //Arrange
        //    RatingViewModel ratingView = new RatingViewModel();
        //    ratingView.GameId = scope.Db.Games.First().Id;

        //    //Act
        //    var initialRatingsCount.
        //    gamesService.RateGame(ratingView);

        //    //Assert

        //    Assert.NotEqual(initialRatingsCount, afterwardRatingsCount);
        //}

        //[Fact]
        //public void RateGame_Should_Update_Rating_If_Null()
        //{
        //    //Arrange
        //    RatingViewModel ratingView = TestData.RatingViewWithoutRatingValue;
        //    var game = scope.Db.Games.First();
        //    ratingView.GameId = game.Id;
            
        //    var initialRatingsCount = scope.Db.Ratings.Count();

        //    //Act
            
        //    gamesService.RateGame(ratingView);
        //    var afterwardRatingsCount = scope.Db.Ratings.Count();

        //    //Assert

        //    Assert.NotEqual(initialRatingsCount, afterwardRatingsCount);
        //}

        [Fact]
        public async Task RateGame_Should_Throw_If_Game_Is_Null()
        {
            //Arrange
            RatingViewModel ratingView = TestData.RatingView;
            ratingView.GameId = Guid.NewGuid();

            //Act
            Func<Task> action = () => gamesService.RateGame(ratingView);

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(action);
        }

    }
}
