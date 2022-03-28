using GameBlog.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace GameBlog.Models
{
    using static GameBlog.Data.DataConstants.User;
    public class UserViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [Range(MinReputation, int.MaxValue, ErrorMessage ="The Reputation must be greater than 0")]
        public int Reputation { get; set; }

        [Required]
        [StringLength(UserNameMaxLength, MinimumLength = UserNameMinLength, ErrorMessage ="The {0} must be between {2} and {1} characters")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public  string Email { get; set; }

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
