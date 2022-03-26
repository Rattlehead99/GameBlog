namespace GameBlog.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.User;

    public class User : IdentityUser<Guid>
    {

        [Required]
        [Range(MinReputation, int.MaxValue)]
        public int Reputation { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}