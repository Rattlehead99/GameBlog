

namespace GameBlog.Controllers
{
    using GameBlog.Data;
    using GameBlog.Data.Models;
    using GameBlog.Models;
    using GameBlog.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Claims;
    using static GameBlog.Data.DataConstants.Role;
   
    [AutoValidateAntiforgeryToken]
    public class ArticlesController : Controller
    {
        private readonly GameBlogDbContext db;
        private readonly UserManager<User> userManager;
        private readonly IArticlesService articlesService;

        public ArticlesController(GameBlogDbContext db, UserManager<User> userManager, IArticlesService articlesService)
        {
            this.db = db;
            this.userManager = userManager;
            this.articlesService = articlesService;
        }

        [Authorize]
        public IActionResult Index(int pageNumber, string searchText)
        {   
            var articles = articlesService.GetAllArticles(pageNumber, searchText);

            return View(articles);
        }

        //GET
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleViewModel article)
        {   
            if (!ModelState.IsValid)
            {
                return View(article);
            }

            await articlesService.CreateArticle(article);

            return RedirectToAction("Index", "Articles");
        }

        //GET
        [Authorize]
        public IActionResult Edit(Guid id)
        {
            var articleView = articlesService.GetArticleById(id);

            return View(articleView);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticleViewModel article)
        {
            if (!ModelState.IsValid)
            {
                return View(article);
            }

            articlesService.EditArticle(article);

            return RedirectToAction("Index", "Articles");
        }

        //GET
        [Authorize]
        public IActionResult Delete(Guid id)
        {
            var articleView = articlesService.GetArticleById(id);

            return View(articleView);
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteForm([FromForm] Guid id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            articlesService.DeleteArticle(id);

            return RedirectToAction("Index", "Articles");
        }

        //GET
        [AllowAnonymous]
        public IActionResult Details(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var articleView = articlesService.Details(id);

            return View(articleView);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostComment(CommentViewModel comment)
        {
            if (!ModelState.IsValid)
            {
                return View(comment);
            }

            var commentData = await articlesService.PostComment(comment);

            return RedirectToAction(nameof(Details), new { id = commentData.ArticleId });
        }

        [HttpPost]
        [Authorize(Roles = Administrator)]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            articlesService.Approve(id);

            return RedirectToAction("Index", "Home");
        }
    }
}


/*
var stream = new MemoryStream();
HttpContext.Request.Body.CopyToAsync(stream);
stream.Seek(0, SeekOrigin.Begin);
Console.WriteLine(new StreamReader(stream).ReadToEnd());

HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
*/