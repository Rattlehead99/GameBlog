
namespace GameBlog.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static DataConstants.Comment;

    public class Comment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(CommentMaxLength)]
        public string Content { get; set; }

        public DateTime PostDate { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public User User { get; set; }

        [ForeignKey(nameof(Article))]
        public Guid ArticleId { get; set; }

        public Article Article { get; set; }
    }
}