namespace GameBlog.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.User;

    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(UserNameMaxLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(PasswordMaxLength)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(FullNameMaxLength)]
        public string FullName { get; set; }

        [Required]
        [Range(MinReputation, int.MaxValue)]
        public int Reputation { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}