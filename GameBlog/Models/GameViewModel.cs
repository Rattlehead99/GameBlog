using GameBlog.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace GameBlog.Models
{
    using static GameBlog.Data.DataConstants.Game;

    public class GameViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage ="The {0} must be between {2} and {1} characters")]
        public string Name { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = "The {0} must be between {2} and {1} characters")]
        public string Description { get; set; }

        [Required]
        [StringLength(GenreMaxLength, MinimumLength = GenreMinLength, ErrorMessage = "The {0} must be between {2} and {1} characters")]
        public string Genre { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
