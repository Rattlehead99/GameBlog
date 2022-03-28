namespace GameBlog.Models
{
    using System.ComponentModel.DataAnnotations;
    using static GameBlog.Data.DataConstants.Comment;

    public class CommentViewModel
    {
        [Required]
        [StringLength(CommentMaxLength, MinimumLength = CommentMinLength, ErrorMessage = "{0} length should be between {2} and {1} characters.")]
        public string Content { get; set; }

        [Required]
        public Guid ArticleId { get; set; }
    }
}
