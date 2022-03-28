using GameBlog.Data.Models;
using System.ComponentModel.DataAnnotations;
using static GameBlog.Data.DataConstants.Article;

namespace GameBlog.Models
{
    //var a =new ArticleViewModel(){
    //Id = 2
//}

    public class ArticleViewModel
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage ="{0} must be betwee {2} and {1} characters.")]
        public string Title { get; set; }

        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage ="The {0} of the Article must be betweeen {2} and {1} characters.")]
        public string Content { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        public Guid UserId { get; set; }

        public bool Approved { get; set; }

       // public Comment NewComment { get; set; } = new Comment();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}