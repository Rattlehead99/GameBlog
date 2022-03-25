using GameBlog.Data.Models;

namespace GameBlog.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public int Reputation { get; set; }

        public string UserName { get; set; }

        public  string Email { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
