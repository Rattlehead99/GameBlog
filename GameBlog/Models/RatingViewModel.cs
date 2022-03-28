namespace GameBlog.Models
{
    using System.ComponentModel.DataAnnotations;
    using static GameBlog.Data.DataConstants.Rating;

    public class RatingViewModel
    {
        public Guid GameId { get; set; }

        [Range(MinRating,MaxRating)]
        [Required]
        public double RatingValue { get; set; }

    }
}
