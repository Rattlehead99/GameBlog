namespace GameBlog.Models
{
    public class CommentViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Content { get; set; }

        public Guid UserId { get; set; }

        public Guid ArticleId { get; set; }
    }
}
