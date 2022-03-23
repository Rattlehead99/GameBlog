namespace GameBlog.Models
{
    public class ArticleListingViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public Guid UserId { get; set; }

        public bool Approved { get; set; }
    }
}
