using GameBlog.Data;
using GameBlog.Data.Models;
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

namespace GameBlog.Test.Services
{
    public class HardwareServiceTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly IHardwareService hardwareService;
        private readonly DependencyScope scope;

        public HardwareServiceTest(CustomWebApplicationFactory factory)
        {
            scope = factory.InitDb();
            hardwareService = scope.ResolveService<IHardwareService>();
        }

        [Fact]
        public void CreateHardware_Should_Add_Hardware_In_DB()
        {
            //Arrange
            HardwareViewModel hardwareView = TestData.HardwareView;

            //Act
            hardwareService.CreateHardware(hardwareView);
            var hardwareCountAfterCreation = scope.Db.Hardware.Count();

            //Assert
            Assert.Equal(2, hardwareCountAfterCreation);
        }

        [Fact]
        public void CreateHardware_Should_Throw_If_ViewModel_Is_Null()
        {
            //Arrange
            HardwareViewModel hardwareView = null;

            //Act
            var action = () => hardwareService.CreateHardware(hardwareView);

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void CreateHardware_Should_Throw_If_Hardware_Type_Is_Wrong()
        {
            //Arrange
            HardwareViewModel hardwareView = TestData.HardwareView;
            hardwareView.Type = "Something";

            //Act
            var action = () => hardwareService.CreateHardware(hardwareView);

            //Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void GetHardwareById_Should_Return_HardwareViewModel()
        {
            //Arrange
            GameBlogDbContext? data = DataBaseMock.Instance;

            var hardwareId = Guid.NewGuid();

            IPaginationService paginationService = new PaginationService(data);

            data.Hardware.Add(new Hardware
            {
                Id = hardwareId,
                Name = "RX 580",
                PerformanceScore = 5200,
                Type = "GPU"
            });
            data.SaveChanges();

            var hardwareService = new HardwareService(paginationService, data);

            //Act
            var result = hardwareService.GetHardwareById(hardwareId);

            //Assert
            Assert.IsType<HardwareViewModel>(result);
            Assert.Equal(hardwareId, result.Id);
        }

        [Fact]
        public void CreateHardware_Twice_Should_Add_Hardware_To_DB()
        {
            hardwareService.CreateHardware(TestData.HardwareViewWithoutId);
            hardwareService.CreateHardware(TestData.HardwareViewWithoutId);

            var hardware = scope.ResolveService<GameBlogDbContext>().Hardware;
            var hardwareCount = hardware.Count();

            Assert.Equal("Gtx 980", hardware.OrderByDescending(x => x.PerformanceScore).First().Name);
            Assert.Equal(3, hardwareCount);
        }

        [Fact]
        public void EditHardware_Should_Throw_When_Hardware_Not_Found()
        {
            //Arrange
            var hardwareView = TestData.HardwareView;

            //Act
            var articles = scope.ResolveService<GameBlogDbContext>().Hardware;
            Action? action = () => hardwareService.EditHardware(hardwareView);

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void EditHardware_Should_Change_Hardware_Data()
        {
            //Arrange
            var hardware = scope.ResolveService<GameBlogDbContext>().Hardware;

            var hardwareView = TestData.HardwareView;
            hardwareService.CreateHardware(hardwareView);

            var initialName = hardwareView.Name;
            var initialPerformance = hardwareView.PerformanceScore;

            var newName = "Something else";
            var newPerformance = 4000;

            hardwareView.Name = newName;
            hardwareView.PerformanceScore = newPerformance;

            //Act
            hardwareService.EditHardware(hardwareView);

            //Assert
            Assert.NotEqual(initialPerformance, hardwareView.PerformanceScore);
            Assert.NotEqual(initialName, hardwareView.Name);

            Assert.Equal(newName, hardwareView.Name);
            Assert.Equal(newPerformance, hardwareView.PerformanceScore);

        }

        [Fact]
        public void DeleteHardware_Should_Remove_Hardware_From_DB()
        {
            //Arrange
            var hardwareView = TestData.HardwareView;

            //Act
            var hardware = scope.ResolveService<GameBlogDbContext>().Hardware;
            hardwareService.CreateHardware(hardwareView);
            var initialCount = hardware.Count();

            hardwareService.DeleteHardware(hardwareView.Id);
            var afterDeleteCount = hardware.Count();

            //Assert
            Assert.NotEqual(afterDeleteCount, initialCount);
        }

        [Fact]
        public void DeleteHardware_Should_Throw_If_Hardware_Not_Found()
        {
            //Arrange
            var hardwareView = TestData.HardwareView;

            //Act
            var articles = scope.ResolveService<GameBlogDbContext>().Articles;
            Action? action = () => hardwareService.DeleteHardware(hardwareView.Id);

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void GetAllHardware_Should_Return_AllHardwareViewModel()
        {
            //Arrange
            int pageNumber = 1;
            string searchText = "";
            var hardware = scope.ResolveService<GameBlogDbContext>().Hardware;

            //Act
            var result = hardwareService.GetAllHardware(pageNumber, searchText);
            var hardwareCount = hardware.Count();
            
            //Assert
            Assert.IsType<AllHardwareViewModel>(result);
            Assert.Equal(hardwareCount, result.HardwareModels.Count());
        }
    }
}
