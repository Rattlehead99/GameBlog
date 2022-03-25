using GameBlog.Data;
using GameBlog.Data.Models;
using GameBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            
            var articleView = new ArticleViewModel
            {
                Approved = article.Approved,
                Comments = article.Comments,
                Content = article.Content,
                Id = id,
                ImageUrl = article.ImageUrl,
                Title = article.Title,
            };
            return View(articleView);
        }

        [HttpPost]
        public IActionResult Edit(ArticleViewModel article)
        {
            if (!ModelState.IsValid)
            {
                return View(article);
            }

            var articleData = db.Articles.Find(article.Id);

            if (articleData == null)
            {
                return NotFound();
            }

            articleData.Approved = article.Approved;
            articleData.Comments = article.Comments;
            articleData.Content = article.Content;
            articleData.ImageUrl = article.ImageUrl;
            articleData.Title = article.Title;

            db.Articles.Update(articleData);
            db.SaveChanges();

            return RedirectToAction("Index", "Articles");
        }

        //GET
        public IActionResult Delete(Guid id)
        {
            Article? article = db.Articles.Find(id);

            if (article == null)
            {
                return NotFound();
            }

            var articleView = new ArticleViewModel
            {
                Approved=article.Approved,
                Comments=article.Comments,
                Content=article.Content,
                Id=id,
                ImageUrl = article.ImageUrl,
                Title=article.Title,
                UserId=id
            };

            return View(articleView);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteForm([FromForm]Guid id) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var articleData = db.Articles.Find(id);

            if (articleData == null)
            {
                return NotFound();
            }

            db.Articles.Remove(articleData);
            db.SaveChanges();

            return RedirectToAction("Index", "Articles");
        }

        //GET
        public IActionResult Details(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var article = db.Articles
                .Include(a => a.Comments)
                .ThenInclude(u => u.User)
                .FirstOrDefault(a => a.Id ==  id);

            if (article == null)
            {
                return NotFound();
            }

            var articleView = new ArticleViewModel
            {
                Approved = article.Approved,
                Comments = article.Comments,
                Content = article.Content,
                Id = id,
                ImageUrl = article.ImageUrl,
                Title = article.Title,
                UserId = id
            };

            return View(articleView);
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