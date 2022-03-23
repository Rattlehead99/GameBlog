
namespace GameBlog.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    using static DataConstants.Article;

    public class Article
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(ContentMaxLength)]
        public string Content { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        public bool Approved { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public User? User { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
