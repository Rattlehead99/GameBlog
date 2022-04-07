using Castle.Core.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace GameBlog.Test.Services
{
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

    public class ArticlesServiceTest : IClassFixture<CustomWebApplicationFactory>
    {
        private GameBlogDbContext db;
        private ArticlesService articleService;
        private readonly CustomWebApplicationFactory factory;
        private readonly AsyncServiceScope scope;

        public ArticlesServiceTest(CustomWebApplicationFactory factory)
        {
            this.CreateService();
            this.factory = factory;
            scope = factory.Services.CreateAsyncScope();
        }

        private T ResolveService<T>()
            where T : notnull
        {
            return scope.ServiceProvider.GetRequiredService<T>();
        }

        private void CreateService()
        {
            //Arrange
            db = DataBaseMock.Instance;

            Mock<IUserStore<User>> userStoreMock = new Mock<IUserStore<User>>();
            IUserStore<User>? userStore = userStoreMock.Object;

            User testUser = new User
            {
                Id = CustomWebApplicationFactory.UserId,
                UserName = "smth@gmail.com",
                Email = "smth@gmail.com",
                NormalizedUserName = "SMTH@GMAIL.COM".Normalize().ToUpperInvariant()
            };

            userStoreMock.Setup(x => x.FindByIdAsync(testUser.Id.ToString(), It.IsAny<CancellationToken>())).ReturnsAsync(testUser);

            UserManager<User> userManager = new UserManager<User>(userStore, null, null, null, null, null, null, null, null);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var claimsPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[] { new Claim(ClaimTypes.NameIdentifier, testUser.Id.ToString()) }
                    )
                );

            mockHttpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

            Assert.Equal(testUser.Id.ToString(), mockHttpContextAccessor.Object.HttpContext.User.Claims.FirstOrDefault().Value);

            db.Articles.Add(new Article
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("{768CCB3A-C7AC-4141-BC00-5348116856E4}"),
                Content = "Bulshiser that should reach 30 symbols at the very least",
                ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
                Title = "Some dumb title"
            });

            db.Users.Add(new User
            {
                Id = CustomWebApplicationFactory.UserId
            });
            db.SaveChanges();

            articleService = new ArticlesService(db, userManager, mockHttpContextAccessor.Object);
        }

        [Fact]
        public async Task CreateArticle_Should_Add_An_Article_In_DB()
        {
            this.CreateService();

            ArticleViewModel articleView = new ArticleViewModel
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("{A96852DC-4582-40FC-975A-7F4FA241357F}"),
                Content = "that should reach 30 symbols at the very least I think it should be so",
                ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
                Title = "Some dumb title"
            };

            //Act
            await articleService.CreateArticle(articleView);
            var articlesCountAfterCreation = db.Articles.Count();

            //Assert
            Assert.Equal(2, articlesCountAfterCreation);
        }

        [Fact]
        public void GetArticleById_Should_Return_ArticleViewModel()
        {
            //Arrange
            GameBlogDbContext? data = DataBaseMock.Instance;

            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            var loggedUser = httpContextAccessor.HttpContext?.User;

            Mock<IUserStore<User>> userStoreMock = new Mock<IUserStore<User>>();
            IUserStore<User>? userStore = userStoreMock.Object;

            UserManager<User>? userManager = UserManagerMock.TestUserManager(userStore);
            var articleId = Guid.NewGuid();

            data.Articles.Add(new Article   
            {
                Id = articleId,
                UserId = Guid.Parse("{768CCB3A-C7AC-4141-BC00-5348116856E4}"),
                Content = "Bulshiser that should reach 30 symbols at the very least",
                ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
                Title = "Some dumb title"
            });
            data.SaveChanges();

            var articleService = new ArticlesService(data, userManager, httpContextAccessor);

            //Act
            var result = articleService.GetArticleById(articleId);

            //Assert
            Assert.IsType<ArticleViewModel>(result);
        }

        [Fact]
        public void GetArticleById_Should_Throw_If_Id_Is_Incorrect()
        {
            //Arrange
            GameBlogDbContext? data = DataBaseMock.Instance;

            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            var loggedUser = httpContextAccessor.HttpContext?.User;

            Mock<IUserStore<User>> userStoreMock = new Mock<IUserStore<User>>();
            IUserStore<User>? userStore = userStoreMock.Object;

            UserManager<User>? userManager = UserManagerMock.TestUserManager(userStore);
            var articleId = Guid.NewGuid();

            data.Articles.Add(new Article
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("{768CCB3A-C7AC-4141-BC00-5348116856E4}"),
                Content = "Bulshiser that should reach 30 symbols at the very least",
                ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
                Title = "Some dumb title"
            });
            data.SaveChanges();

            var articleService = new ArticlesService(data, userManager, httpContextAccessor);

            //Assert
            Assert.Throws<ArgumentNullException>(() => articleService.GetArticleById(articleId));
        }

        [Fact]
        public void Details_Should_Return_ArticleViewModel()
        {
            //Arrange
            GameBlogDbContext? data = DataBaseMock.Instance;

            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            var loggedUser = httpContextAccessor.HttpContext?.User;

            Mock<IUserStore<User>> userStoreMock = new Mock<IUserStore<User>>();
            IUserStore<User>? userStore = userStoreMock.Object;

            UserManager<User>? userManager = UserManagerMock.TestUserManager(userStore);
            var articleId = Guid.NewGuid();

            data.Articles.Add(new Article
            {
                Id = articleId,
                UserId = Guid.Parse("{768CCB3A-C7AC-4141-BC00-5348116856E4}"),
                Content = "Bulshiser that should reach 30 symbols at the very least",
                ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
                Title = "Some dumb title"
            });
            data.SaveChanges();

            var articleService = new ArticlesService(data, userManager, httpContextAccessor);

            //Act
            var details = articleService.Details(articleId);

            //Assert
            Assert.IsType<ArticleViewModel>(details);
        }
        #region TODO: PostComment returns CommentViewModel
        //[Fact]
        //public async Task PostComment_Should_Return_CommentViewModel()
        //{
        //    //Arrange
        //    GameBlogDbContext? data = DataBaseMock.Instance;

        //    IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
        //    var loggedUser = httpContextAccessor.HttpContext?.User;

        //    Mock<IUserStore<User>> userStoreMock = new Mock<IUserStore<User>>();
        //    IUserStore<User>? userStore = userStoreMock.Object;

        //    UserManager<User>? userManager = UserManagerMock.TestUserManager(userStore);
        //    var articleId = Guid.NewGuid();

        //    CommentViewModel commentView = new CommentViewModel
        //    {
        //        ArticleId = articleId,
        //        Content = "Comment Test"
        //    };

        //    data.Articles.Add(new Article
        //    {
        //        Id = articleId,
        //        UserId = Guid.Parse("{768CCB3A-C7AC-4141-BC00-5348116856E4}"),
        //        Content = "Bulshiser that should reach 30 symbols at the very least",
        //        ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
        //        Title = "Some dumb title",
        //        Comments = new List<Comment>
        //        {
        //            new Comment { Id = articleId, Content = "TestComment"}
        //        }
        //    });
        //    data.SaveChanges();

        //    var articleService = new ArticlesService(data, userManager, httpContextAccessor);

        //    //Act
        //    var commentViewData = await articleService.PostComment(commentView);

        //    //Assert
        //    Assert.IsType<CommentViewModel>(commentViewData);

        //}
        #endregion

        [Fact]
        public async Task Create_Should_Add_Article_To_DB()
        {
            var articleService = this.ResolveService<IArticlesService>();

            await articleService.CreateArticle(new ArticleViewModel()
            {
                Content = "Bulshiser that should reach 30 symbols at the very leasfdhhgfcdsvhfsdgsdfgdfsgdsfgsdfgdfst",
                ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
                Title = "Some dumb title123123",
                Approved = false
            });

            var articles = ResolveService<GameBlogDbContext>().Articles;
            var articleCount = articles.Count();

            Assert.Equal("Some dumb title123123", articles.OrderByDescending(x=>x.PostDate).First().Title);
            Assert.Equal(2, articleCount);
        }

        [Fact]
        public async void EditArticle_Should_Throw_When_Article_Not_Found()
        {
            //Arrange
            var articleService = this.ResolveService<IArticlesService>();
            var articleView = new ArticleViewModel()
            {
                Content = "Bulshiser that should reach 30 symbols at the very leasfdhhgfcdsvhfsdgsdfgdfsgdsfgsdfgdfst",
                ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
                Title = "Some dumb title123123",
                Approved = false
            };
          
            //Act
            var articles = ResolveService<GameBlogDbContext>().Articles;
            Action? action = () => articleService.EditArticle(articleView);

            //Assert
            Assert.Throws<ArgumentNullException>(action);

        }

        [Fact]
        public void DeleteArticle_Should_Throw_If_Article_Not_Found()
        {
            //Arrange
            var articleSerivce = this.ResolveService<IArticlesService>();
            var articleView = new ArticleViewModel()
            {
                Content = "Bulshiser that should reach 30 symbols at the very leasfdhhgfcdsvhfsdgsdfgdfsgdsfgsdfgdfst",
                ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
                Title = "Some dumb title123123",
                Approved = false
            };

            //Act
            var articles = ResolveService<GameBlogDbContext>().Articles;
            Action? action = () => articleSerivce.DeleteArticle(articleView.Id);

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void DeleteArticle_Should_Delete_Article_From_DB()
        {
            //Arrange
            var articleSerivce = this.ResolveService<IArticlesService>();
            var articleView = new ArticleViewModel()
            {
                Content = "Bulshiser that should reach 30 symbols at the very leasfdhhgfcdsvhfsdgsdfgdfsgdsfgsdfgdfst",
                ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
                Title = "Some dumb title123123",
                Approved = false
            };
            
            //Act
            var articles = ResolveService<GameBlogDbContext>().Articles;
            articleSerivce.CreateArticle(articleView);
            var initialArticlesCount = articles.Count();

            articleSerivce.DeleteArticle(articleView.Id);
            var afterDeleteArticlesCount = articles.Count();

            //Assert
            Assert.NotEqual(afterDeleteArticlesCount, initialArticlesCount);
        }

        [Fact]
        public void Approve_Should_Throw_If_Article_Is_Null()
        {
            //Arrange
            var articleSerivce = this.ResolveService<IArticlesService>();
            var articleView = new ArticleViewModel()
            {
                Content = "Bulshiser that should reach 30 symbols at the very leasfdhhgfcdsvhfsdgsdfgdfsgdsfgsdfgdfst",
                ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
                Title = "Some dumb title123123",
                Approved = false
            };

            //Act
            var articles = ResolveService<GameBlogDbContext>().Articles;
            var action = () => articleSerivce.Approve(articleView.Id);

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void PostComment_Should_Throw_When_ArticleData_Does_Not_Exist()
        {
            //Arrange
            var articleSerivce = this.ResolveService<IArticlesService>();
            var articleView = new ArticleViewModel()
            {
                Content = "Bulshiser that should reach 30 symbols at the very leasfdhhgfcdsvhfsdgsdfgdfsgdsfgsdfgdfst",
                ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
                Title = "Some dumb title123123",
                Approved = false
            };
            CommentViewModel commentView = new CommentViewModel
            {
                ArticleId = articleView.Id,
                Content = "Comment Test"
            };

            //Act
            var action = () => articleSerivce.PostComment(commentView);

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(action);

        }
    }
}

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        Microsoft.Extensions.Logging.ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] { new Claim(ClaimTypes.Name, "Test user"), new Claim(ClaimTypes.Role, "Administrator") };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}