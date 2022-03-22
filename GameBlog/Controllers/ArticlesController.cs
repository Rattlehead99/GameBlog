using GameBlog.Data;
using GameBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameBlog.Controllers
{

    public class ArticlesController : Controller
    {
        private readonly GameBlogDbContext db;

        public ArticlesController(GameBlogDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var articlesQuery = db.Articles.AsQueryable();

            var articles = articlesQuery.Select(a => new ArticleListingViewModel
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
    }
}
