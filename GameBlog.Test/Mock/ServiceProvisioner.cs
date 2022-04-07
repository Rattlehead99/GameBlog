using Castle.Core.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore.InMemory;
using GameBlog.Services;
using GameBlog.Test.Mock;
using GameBlog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using GameBlog.Models;
using GameBlog.Data;
using System.Threading;
using System.Security.Principal;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Net.Http;

namespace GameBlog.Test.Mock
{
    public class ServiceProvisioner : IClassFixture<CustomWebApplicationFactory>
    {
        private GameBlogDbContext db;
        private ArticlesService articleService;
        private readonly  CustomWebApplicationFactory factory;
        private readonly AsyncServiceScope scope;

        public ServiceProvisioner(CustomWebApplicationFactory factor)
        {
            this.factory = factor;
            scope = factory.Services.CreateAsyncScope();
        }
        //public static void CreateService()
        //{
        //    //Arrange
        //    db = DataBaseMock.Instance;

        //    Mock<IUserStore<User>> userStoreMock = new Mock<IUserStore<User>>();
        //    IUserStore<User>? userStore = userStoreMock.Object;

        //    User testUser = new User
        //    {
        //        Id = CustomWebApplicationFactory.UserId,
        //        UserName = "smth@gmail.com",
        //        Email = "smth@gmail.com",
        //        NormalizedUserName = "SMTH@GMAIL.COM".Normalize().ToUpperInvariant()
        //    };

        //    userStoreMock.Setup(x => x.FindByIdAsync(testUser.Id.ToString(), It.IsAny<CancellationToken>())).ReturnsAsync(testUser);

        //    UserManager<User> userManager = new UserManager<User>(userStore, null, null, null, null, null, null, null, null);

        //    var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        //    var claimsPrincipal = new ClaimsPrincipal(
        //        new ClaimsIdentity(
        //            new Claim[] { new Claim(ClaimTypes.NameIdentifier, testUser.Id.ToString()) }
        //            )
        //        );

        //    mockHttpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

        //    Assert.Equal(testUser.Id.ToString(), mockHttpContextAccessor.Object.HttpContext.User.Claims.FirstOrDefault().Value);

        //    db.Articles.Add(new Article
        //    {
        //        Id = Guid.NewGuid(),
        //        UserId = Guid.Parse("{768CCB3A-C7AC-4141-BC00-5348116856E4}"),
        //        Content = "Bulshiser that should reach 30 symbols at the very least",
        //        ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
        //        Title = "Some dumb title"
        //    });

        //    db.Users.Add(new User
        //    {
        //        Id = CustomWebApplicationFactory.UserId
        //    });
        //    db.SaveChanges();

        //    articleService = new ArticlesService(db, userManager, mockHttpContextAccessor.Object);
        //}
    }
}
