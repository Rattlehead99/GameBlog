namespace GameBlog.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Game;

    public class Game
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [StringLength(GenreMaxLength)]
        public string Genre { get; set; }

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}