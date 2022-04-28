namespace GameBlog.Models
{
    using System.ComponentModel.DataAnnotations;
    using static GameBlog.Data.DataConstants.Hardware;
    public class HardwareViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage ="{0} should be between {2} and {1} characters.")]
        public string Name { get; set; }

        [Range(MinPerformance, MaxPerformance, ErrorMessage = "{0} should be between {1} and {2} score.")]
        public int PerformanceScore { get; set; }

        [StringLength(TypeMaxLength, MinimumLength = TypeMinLength, ErrorMessage ="{0} must be either CPU or GPU and {1} characters long.")]
        public string Type { get; set; }
    }
}
