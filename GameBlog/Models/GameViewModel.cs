using GameBlog.Data.Models;

namespace GameBlog.Models
{
    public class GameViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string Description { get; set; }

        public string Genre { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
