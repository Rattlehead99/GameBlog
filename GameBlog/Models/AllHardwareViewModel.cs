namespace GameBlog.Models
{
    public class AllHardwareViewModel
    {
        public IEnumerable<HardwareViewModel> HardwareModels { get; set; } = new List<HardwareViewModel>();

        public int PageNumber { get; set; }
    }
}
