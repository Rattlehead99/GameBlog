using GameBlog.Models;
using GameBlog.Services;
using GameBlog.Test.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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
    }
}
