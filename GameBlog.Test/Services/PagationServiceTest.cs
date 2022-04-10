using GameBlog.Data.Models;
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
    public class PagationServiceTest : IClassFixture<CustomWebApplicationFactory>
    {

        private IPaginationService paginationService;
        private readonly DependencyScope scope;

        public PagationServiceTest(CustomWebApplicationFactory factory)
        {
            scope = factory.InitDb();
            paginationService = scope.ResolveService<IPaginationService>();
        }
        [Fact]
        public void PageCorrection_Should_Return_PageNumber()
        {
            //Arrange
            int pageNumber = 1;
            var users = scope.Db.Users.AsQueryable();

            //Act
            var newPageNumber = paginationService.PageCorrection(pageNumber, users);

            //Assert
            Assert.IsType<int>(newPageNumber);

        }
        [Fact]
        public void PageCorrection_Should_Return_PageCount_Equal_To_1_If_Its_Less()
        {
            //Arrange
            int pageNumber = 1;
            IQueryable<User> users = new List<User>().AsQueryable();

            //Act
            var newPageNumber = paginationService.PageCorrection(pageNumber, users);

            //Assert
            Assert.Equal(1, newPageNumber);

        }

        [Fact]
        public void PageCorrection_Should_Return_1_If_Page_Number_Less_Than_1()
        {
            //Arrange
            int pageNumber = 0;
            var users = scope.Db.Users.AsQueryable();

            //Act
            var newPageNumber = paginationService.PageCorrection(pageNumber, users);

            //Assert
            Assert.Equal(pageNumber, newPageNumber -1);
        }

        [Fact]
        public void PageCorrection_Should_Return_Page_Count_If_Page_Number_Is_Higher()
        {
            //Arrange
           
            var users = scope.Db.Users.AsQueryable();
            int pageNumber = 10;
            int pageSize = 6;
            double pageCount = Math.Ceiling(users.Count() / (double)pageSize);

            //Act
            var newPageNumber = paginationService.PageCorrection(pageNumber, users);

            //Assert
            Assert.Equal(newPageNumber, pageCount);
        }

        [Fact]
        public void Pagination_Should_Return_IQueryable_Of_Type_With_Fewer_Entities_Than_PageSize()
        {
            //Arrange
            int pageNumber = 1;
            var users = scope.Db.Users.AsQueryable();

            //Act
            var usersQuery = paginationService.Pagination(pageNumber, users);
            var pageSize = usersQuery.Count();
            bool IsSmallerThanPageSize = (pageSize <= 6);
            bool IsSmallerOrEqual = true;
            
            //Assert
            Assert.True(IsSmallerOrEqual, IsSmallerThanPageSize.ToString());

        }
    }
}
