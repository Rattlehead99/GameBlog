namespace GameBlog.Services
{
    using GameBlog.Data.Models;
    using GameBlog.Models;
    using Microsoft.AspNetCore.Mvc;

    public interface IGamesService
    {
        public AllGamesViewModel GetAllGames(int pageNumber, string searchText, string sortOrder);

        public void CreateGame(GameViewModel game);

        public GameViewModel GetGameById(Guid id);

        public void EditGame(GameViewModel game);

        public void DeleteGame([FromForm]Guid id);

        public GameViewModel Details(Guid id);

        public Task RateGame(RatingViewModel rating);
    }
}
