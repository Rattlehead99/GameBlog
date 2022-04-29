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

        [Required]
        [StringLength(NameMaxLength)]
        public string Name { get; set; }

        [NotMapped]
        public ICollection<UserReputations> UserReputations { get; set; } = new List<UserReputations>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}