namespace GameBlog.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static DataConstants.Rating;

    public class Rating
    { 
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(Game))]
        public Guid GameId { get; set; }
        public Game Game { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Range(MinRating, int.MaxValue)]
        public double RatingValue { get; set; } 
    }
}