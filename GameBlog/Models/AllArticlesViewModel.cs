namespace GameBlog.Models
{
    public class AllArticlesViewModel
    {
        public string Search { get; set; }

        public IEnumerable<ArticleListingViewModel> Articles { get; set; } = new List<ArticleListingViewModel>();
    }
}
