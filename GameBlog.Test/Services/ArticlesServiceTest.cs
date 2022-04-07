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
    using GameBlog.Test.TestConstants;

    public class ArticlesServiceTest : IClassFixture<CustomWebApplicationFactory>
    {
        private IArticlesService articleService;
        private readonly DependencyScope scope;

        public ArticlesServiceTest(CustomWebApplicationFactory factory)
        {
            scope = factory.InitDb();
            articleService = scope.ResolveService<IArticlesService>(); 
        }

        [Fact]
        public async Task CreateArticle_Should_Add_An_Article_In_DB()
        {

            ArticleViewModel articleView = TestData.ArticleView;

            //Act
            await articleService.CreateArticle(articleView);
            var articlesCountAfterCreation = scope.Db.Articles.Count();

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
            var articleService = scope.ResolveService<IArticlesService>();

            await articleService.CreateArticle(TestData.ArticleView);

            var articles = scope.ResolveService<GameBlogDbContext>().Articles;
            var articleCount = articles.Count();

            Assert.Equal("Some dumb title123123", articles.OrderByDescending(x=>x.PostDate).First().Title);
            Assert.Equal(2, articleCount);
        }

        [Fact]
        public async Task Create_Twice_Should_Add_Article_To_DB()
        {
            await articleService.CreateArticle(TestData.ArticleView);
            await articleService.CreateArticle(TestData.ArticleView);

            var articles = scope.ResolveService<GameBlogDbContext>().Articles;
            var articleCount = articles.Count();

            Assert.Equal("Some dumb title123123", articles.OrderByDescending(x => x.PostDate).First().Title);
            Assert.Equal(3, articleCount);
        }

        [Fact]
        public void EditArticle_Should_Throw_When_Article_Not_Found()
        {
            //Arrange
            var articleView = TestData.ArticleView;
          
            //Act
            var articles = scope.ResolveService<GameBlogDbContext>().Articles;
            Action? action = () => articleService.EditArticle(articleView);

            //Assert
            Assert.Throws<ArgumentNullException>(action);

        }

        [Fact]
        public void DeleteArticle_Should_Throw_If_Article_Not_Found()
        {
            //Arrange
            var articleView = TestData.ArticleView;

            //Act
            var articles = scope.ResolveService<GameBlogDbContext>().Articles;
            Action? action = () => articleService.DeleteArticle(articleView.Id);

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void DeleteArticle_Should_Delete_Article_From_DB()
        {
            //Arrange
            var articleView = TestData.ArticleView;
            
            //Act
            var articles = scope.ResolveService<GameBlogDbContext>().Articles;
            articleService.CreateArticle(articleView);
            var initialArticlesCount = articles.Count();

            articleService.DeleteArticle(articleView.Id);
            var afterDeleteArticlesCount = articles.Count();

            //Assert
            Assert.NotEqual(afterDeleteArticlesCount, initialArticlesCount);
        }

        [Fact]
        public void Approve_Should_Throw_If_Article_Is_Null()
        {
            //Arrange
            var articleView = TestData.ArticleView;

            //Act
            var articles = scope.ResolveService<GameBlogDbContext>().Articles;
            var action = () => articleService.Approve(articleView.Id);

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void Approve_Should_Change_Article_Property()
        {
            //Arrange
            var articles = scope.ResolveService<GameBlogDbContext>().Articles;

            var articleView = TestData.ArticleViewWithId;
            articleService.CreateArticle(articleView);

            bool isApproved = true;
            bool isApprovedInitially = articleView.Approved;

            articleService.Approve(articleView.Id);

            var newArticleView = articles.FirstOrDefault(x => x.Id == articleView.Id);

            Assert.Equal(isApproved, newArticleView.Approved);
        }

        [Fact]
        public void PostComment_Should_Throw_When_ArticleData_Does_Not_Exist()
        {
            //Arrange
            var articleService = scope.ResolveService<IArticlesService>();
            var articleView = TestData.ArticleView;

            CommentViewModel commentView = new CommentViewModel
            {
                ArticleId = articleView.Id,
                Content = "Comment Test"
            };

            //Act
            var action = () => articleService.PostComment(commentView);

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(action);

        }

        [Fact]
        public void EditArticle_Should_Change_Article_Data()
        {
            //Arrange
            var articleService = scope.ResolveService<IArticlesService>();
            var articles = scope.ResolveService<GameBlogDbContext>().Articles;

            var articleView = TestData.ArticleViewWithId;
            articleService.CreateArticle(articleView);

            var initialTitle = articleView.Title;
            var initialContent = articleView.Content;

            var newTitle = "Something different";
            var newContent = "More stuff to fill 30 characters with";

            articleView.Title = newTitle;
            articleView.Content = newContent;

            //Act
            articleService.EditArticle(articleView);

            //Assert
            Assert.NotEqual(initialContent, articleView.Content);
            Assert.NotEqual(initialTitle, articleView.Title);

            Assert.Equal(newTitle, articleView.Title);
            Assert.Equal(newContent, articleView.Content);

           
        }
    }
}
