namespace GameBlog.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static GameBlog.Data.DataConstants.Hardware;

    public class Hardware
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(NameMaxLength)]
        public string Name { get; set; }

        [Range(MinPerformance, MaxPerformance)]
        public int PerformanceScore { get; set; }

        [Required]
        [StringLength(TypeMaxLength)]
        public string Type { get; set; }
    }
}
