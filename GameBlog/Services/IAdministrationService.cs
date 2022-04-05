namespace GameBlog.Services
{
    using GameBlog.Data;
    using GameBlog.Data.Models;
    using GameBlog.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using static Data.DataConstants.Role;

    public interface IAdministrationService
    {
        public AllUsersViewModel AllUsers(string searchText);

        public UserViewModel AdministratedUser(Guid id);

        public Task ChangeUserRole(Guid id);
    }
}