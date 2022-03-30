using GameBlog.Data;
using GameBlog.Data.Models;
using GameBlog.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GameBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GameBlogDbContext db;

        public HomeController(ILogger<HomeController> logger, GameBlogDbContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public IActionResult Index(string searchText)
        {
            var articlesQuery = db.Articles.AsQueryable();

            if (!String.IsNullOrEmpty(searchText))
            {
                articlesQuery = articlesQuery
                    .Where(s => s.Title.Contains(searchText) || s.Content.Contains(searchText));
            }

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
                Articles = articles
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}