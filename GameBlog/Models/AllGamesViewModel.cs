namespace GameBlog.Models
{
    public class AllGamesViewModel
    {
        public IEnumerable<GameViewModel> Games { get; set; } = new List<GameViewModel>();

        public int PageNumber { get; set; }
    }
}
