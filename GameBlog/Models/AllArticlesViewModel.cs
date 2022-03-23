namespace GameBlog.Models
{
    public class AllArticlesViewModel
    {
        public string Search { get; set; }

        public IEnumerable<ArticleViewModel> Articles { get; set; } = new List<ArticleViewModel>();
    }
}
