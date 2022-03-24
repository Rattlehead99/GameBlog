namespace GameBlog.Models
{
    public class AllGamesViewModel
    {
        public string Search { get; set; }

        public IEnumerable<GameViewModel> Games { get; set; } = new List<GameViewModel>();
    }
}
