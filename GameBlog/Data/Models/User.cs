namespace GameBlog.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static DataConstants.User;

    public class User : IdentityUser<Guid>
    {

        [Required]
        [Range(MinReputation, int.MaxValue)]
        public int Reputation { get; set; }

        [NotMapped]
        public ICollection<string> ReputationLikes { get; set; } = new List<string>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}