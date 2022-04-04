using GameBlog.Data;
using GameBlog.Data.Models;
using GameBlog.Models;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        public IActionResult Index(int pageNumber = 1, string searchText ="")
        {
            int pageSize = 6;
            double pageCount = Math.Ceiling(db.Articles.Count()/ (double)pageSize);

            if (pageNumber < 1)
            {
                return RedirectToAction("Index", new { pageNumber = 1 });
            }
            if (pageNumber > pageCount)
            {
                return RedirectToAction("Index", new {pageNumber = pageCount });
            }

            IQueryable<Article>? articlesQuery = db.Articles
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsQueryable();
          
            if (!String.IsNullOrEmpty(searchText))
            {
                articlesQuery = articlesQuery
                    .Where(s => s.Title.Contains(searchText) || s.Content.Contains(searchText));
            }

            List<ArticleViewModel>? articles = articlesQuery.Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Content = a.Content,
                Title = a.Title,
                ImageUrl = a.ImageUrl,
                UserId = a.UserId,
                Approved = a.Approved,
            })
            .ToList();


            return View(new AllArticlesViewModel
            {
                Articles = articles,
                PageNumber = pageNumber
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}