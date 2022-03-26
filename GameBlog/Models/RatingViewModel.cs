namespace GameBlog.Models
{
    public class RatingViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid GameId { get; set; }

        public Guid UserId { get; set; }

        public double RatingValue { get; set; }

    }
}
