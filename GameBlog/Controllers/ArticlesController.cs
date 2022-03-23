using GameBlog.Data;
using GameBlog.Data.Models;
using GameBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameBlog.Controllers
{

    public class ArticlesController : Controller
    {
        private readonly GameBlogDbContext db;
        private readonly UserManager<User> userManager;

        public ArticlesController(GameBlogDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var articlesQuery = db.Articles.AsQueryable();

            var articles = articlesQuery.Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Content = a.Content,
                Title = a.Title,
                ImageUrl = a.ImageUrl,
                UserId = a.UserId,
                Approved = a.Approved
            })
            .ToList();

            return View(new AllArticlesViewModel
            {
                Search = "null",
                Articles = articles
            });
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticleViewModel article)
        {

            var user = await userManager.GetUserAsync(User);
        

            if (!ModelState.IsValid)
            {
                return View(article);
            }

            var articleData = new Article
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Comments = article.Comments,
                Approved = false,
                ImageUrl = article.ImageUrl,
                UserId = user.Id
            };

            db.Articles.Add(articleData);
            db.SaveChanges();

            return RedirectToAction("Index", "Articles");
        }

        //GET
        public IActionResult Edit(Guid id)
        {
            Article? article = db.Articles.SingleOrDefault(a => a.Id == id);

            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ArticleViewModel article)
        {
            if (!ModelState.IsValid)
            {
                return View(article);
            }

            var user = await userManager.GetUserAsync(User);

            var articleData = new Article
            {
                Id = article.Id,
                Approved = false,
                Comments = article.Comments,
                Content = article.Content,
                ImageUrl = article.ImageUrl,
                Title = article.Title,
                UserId = user.Id
            };

            db.Articles.Update(articleData);
            db.SaveChanges();

            return RedirectToAction("Index", "Articles");
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