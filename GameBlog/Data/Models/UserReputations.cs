namespace GameBlog.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static DataConstants.User;

    public class UserReputations
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Range(MinReputation, int.MaxValue)]
        public int Reputation { get; set; }
        
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(LikedUser))]
        public Guid LikedUserId { get; set; }
        public User LikedUser { get; set; }
    }
}
