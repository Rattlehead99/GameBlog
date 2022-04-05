

namespace GameBlog.Services
{
    using GameBlog.Data.Models;
    using GameBlog.Models;

    public interface IUsersService
    {
        public AllUsersViewModel GetAllUsers(int pageNumber, string searchText);

        public Task<UserViewModel> GetUserProfile();

        public UserViewModel Profile(Guid id);

        public Task Rate(Guid id);

        public User GetUserById(Guid id);
    }
}
