using GameBlog.Data.Models;

namespace GameBlog.Models
{
    //var a =new ArticleViewModel(){
    //Id = 2
//}

    public class ArticleViewModel
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public Guid UserId { get; set; }

        public bool Approved { get; set; }

        public Comment NewComment { get; set; } = new Comment();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}