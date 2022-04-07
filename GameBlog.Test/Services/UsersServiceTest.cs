using GameBlog.Models;
using GameBlog.Services;
using GameBlog.Test.Mock;
using GameBlog.Test.TestConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using GameBlog.Data.Models;

namespace GameBlog.Test.Services
{
    public class UsersServiceTest : IClassFixture<CustomWebApplicationFactory>
    {
        private IUsersService usersService;
        private readonly DependencyScope scope;

        public UsersServiceTest(CustomWebApplicationFactory factory)
        {
            scope = factory.InitDb();
            usersService = scope.ResolveService<IUsersService>();
        }

        [Fact]
        public void GetAllUsers_Should_Return_AllUsersViewModel()
        {
            //Arrange
            int pageNumber = 1;
            string searchResult = "";

            //Act
            var result = usersService.GetAllUsers(pageNumber, searchResult);

            //Assert
            Assert.IsType<AllUsersViewModel>(result);
        }

        [Fact]
        public void GetUserProfile_Should_Return_UserViewModel_With_Properties()
        {
            //Arrange
            var user = scope.Db.Users.First();


        }
        [Fact]
        public void GetUserById_Should_Return_User()
        {
            //Arrange
            User user = scope.Db.Users.First();

            //Act
            var result = usersService.GetUserById(user.Id);

            //Assert
            Assert.IsType<User>(result);
        }

        [Fact]
        public void GetUserById_Should_Throw_If_User_Is_Null()
        {
            //Arrange
            User user = new User();

            //Act
            var action = () => usersService.GetUserById(user.Id);

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

    }
}
