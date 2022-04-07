using GameBlog.Test.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using GameBlog.Services;
using GameBlog.Test.TestConstants;
using GameBlog.Models;
using GameBlog.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GameBlog.Test.Services
{
    public class AdministrationServiceTest : IClassFixture<CustomWebApplicationFactory>
    {
        private IAdministrationService administraionService;
        private readonly DependencyScope scope;

        public AdministrationServiceTest(CustomWebApplicationFactory factory)
        {
            scope = factory.InitDb();
            administraionService = scope.ResolveService<IAdministrationService>();
        }

        [Fact]
        public void AdministratedUser_Should_Return_UserViewModel()
        {
            //Arrange
            var user = scope.Db.Users.First();

            //Act
            var result = administraionService.AdministratedUser(user.Id);

            //Assert
            Assert.IsType<UserViewModel>(result);
        }

        [Fact]
        public void AdministratedUser_Should_Throw_If_User_Is_Null()
        {
            //Arrange
            var user = new User();

            //Act
            var action = () => administraionService.AdministratedUser(user.Id);

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void AllUsersViewModel_Should_Return_AllUsersViewModel()
        {
            //Arrange
            int pageNumber = 1;
            string searchText = "";

            //Act
            var userView = administraionService.AllUsers(searchText, pageNumber);

            //Assert
            Assert.IsType<AllUsersViewModel>(userView);
        }

        [Fact]
        public async Task ChangeUserRole_Should_Change_The_Role_Of_User()
        {
            //Arrange
            var httpContext = scope.ResolveService<IHttpContextAccessor>();
            var userManager = scope.ResolveService<UserManager<User>>();
            var user = await userManager.GetUserAsync(httpContext.HttpContext?.User);
            var initialRole = userManager.IsInRoleAsync(user, "Administrator");

            //Act
            await administraionService.ChangeUserRole(user.Id);
            var newRole = userManager.IsInRoleAsync(user, "Administrator");


            //Assert
            Assert.NotEqual(initialRole, newRole);
        }

        [Fact]
        public async Task ChangeUserRole_Should_Remove_The_Role_Of_User()
        {
            //Arrange
            var httpContext = scope.ResolveService<IHttpContextAccessor>();
            var userManager = scope.ResolveService<UserManager<User>>();
            var user = await userManager.GetUserAsync(httpContext.HttpContext?.User);
            var initialRole = userManager.IsInRoleAsync(user, "Administrator");

            //Act
            await administraionService.ChangeUserRole(user.Id);
            await administraionService.ChangeUserRole(user.Id);
            var newRole = userManager.IsInRoleAsync(user, "Administrator");


            //Assert
            Assert.Equal(initialRole, newRole);
        }

        [Fact]
        public async Task ChangeUserRole_Should_Throw_If_User_Is_Null()
        {
            //Arrange
            User user = new User();

            //Act
            Func<Task> task = () => administraionService.ChangeUserRole(user.Id);

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(task);
        }
    }
}
