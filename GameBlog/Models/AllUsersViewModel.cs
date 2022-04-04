namespace GameBlog.Models
{
    public class AllUsersViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; } = new List<UserViewModel>();

        public int PageNumber { get; set; }
    }
}
