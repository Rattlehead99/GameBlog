﻿

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
        public IActionResult Index(int pageNumber = 1, string searchText = "")
        {

            //int pageSize = 6;
            //double pageCount = Math.Ceiling(db.Articles.Count() / (double)pageSize);

            //if (pageNumber < 1)
            //{
            //    return RedirectToAction("Index", new { pageNumber = 1 });
            //}
            //if (pageNumber > pageCount)
            //{
            //    return RedirectToAction("Index", new { pageNumber = pageCount });
            //}

            //var articlesQuery = db.Articles
            //    .Skip((pageNumber - 1) * pageSize)
            //    .Take(pageSize)
            //    .OrderByDescending(a => a.PostDate).AsQueryable();

            //if (!String.IsNullOrEmpty(searchText))
            //{
            //    articlesQuery = articlesQuery
            //        .Where(s => s.Title.Contains(searchText) || s.Content.Contains(searchText));
            //}

            //var articles = articlesQuery.Select(a => new ArticleViewModel
            //{
            //    Id = a.Id,
            //    Content = a.Content,
            //    Title = a.Title,
            //    ImageUrl = a.ImageUrl,
            //    UserId = a.UserId,
            //    Approved = a.Approved
            //})
            //.ToList();

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
        public async Task<IActionResult> Create(ArticleViewModel article)
        {
            //User? user = await userManager.GetUserAsync(User);

            //Article? articleData = new Article
            //{
            //    Id = article.Id,
            //    Title = article.Title,
            //    Content = article.Content,
            //    Comments = article.Comments,
            //    Approved = false,
            //    ImageUrl = article.ImageUrl,
            //    UserId = user.Id
            //};

            //db.Articles.Add(articleData);
            //db.SaveChanges();

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
            //Article? article = db.Articles.SingleOrDefault(a => a.Id == id);

            //var articleView = new ArticleViewModel
            //{
            //    Approved = article.Approved,
            //    Comments = article.Comments,
            //    Content = article.Content,
            //    Id = id,
            //    ImageUrl = article.ImageUrl,
            //    Title = article.Title,
            //};

            var articleView = articlesService.GetArticleById(id);

            return View(articleView);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(ArticleViewModel article)
        {
            //var articleData = db.Articles.Find(article.Id);

            //if (articleData == null)
            //{
            //    return NotFound();
            //}

            //articleData.Approved = article.Approved;
            //articleData.Comments = article.Comments;
            //articleData.Content = article.Content;
            //articleData.ImageUrl = article.ImageUrl;
            //articleData.Title = article.Title;

            //db.Articles.Update(articleData);
            //db.SaveChanges();

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
            //Article? article = db.Articles.Find(id);

            //if (article == null)
            //{
            //    return NotFound();
            //}

            //var articleView = new ArticleViewModel
            //{
            //    Approved = article.Approved,
            //    Comments = article.Comments,
            //    Content = article.Content,
            //    Id = id,
            //    ImageUrl = article.ImageUrl,
            //    Title = article.Title,
            //    UserId = id
            //};

            var articleView = articlesService.GetArticleById(id);

            return View(articleView);
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize]
        public IActionResult DeleteForm([FromForm] Guid id)
        {
            //var articleData = db.Articles.Find(id);

            //if (articleData == null)
            //{
            //    return NotFound();
            //}

            //db.Articles.Remove(articleData);
            //db.SaveChanges();

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
            //var article = db.Articles
            //    .Include(a => a.Comments)
            //    .ThenInclude(u => u.User)
            //    .FirstOrDefault(a => a.Id == id);

            //if (article == null)
            //{
            //    return NotFound();
            //}

            //var articleView = new ArticleViewModel
            //{
            //    Approved = article.Approved,
            //    Comments = article.Comments.OrderByDescending(c => c.PostDate).ToList(),
            //    Content = article.Content,
            //    Id = id,
            //    ImageUrl = article.ImageUrl,
            //    Title = article.Title,
            //    UserId = id
            //};

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var articleView = articlesService.Details(id);

            return View(articleView);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostComment(CommentViewModel comment)
        {

            //User? user = await userManager.GetUserAsync(User);

            //bool articleData = db.Articles
            //    .Any(a => a.Id == comment.ArticleId);

            //if (!articleData)
            //{
            //    return NotFound();
            //}

            //Comment? commentData = new Comment
            //{
            //    Id = comment.ArticleId,
            //    ArticleId = comment.ArticleId,
            //    UserId = user.Id,
            //    Content = comment.Content
            //};

            //db.Comments.Add(commentData);
            //db.SaveChanges();

            if (!ModelState.IsValid)
            {
                return View(comment);
            }

            var commentData = await articlesService.PostComment(comment);

            return RedirectToAction(nameof(Details), new { id = commentData.ArticleId });
        }

        [HttpPost]
        [Authorize(Roles = Administrator)]
        public IActionResult Approve(Guid id)
        {
            //var article = db.Articles
            //    .Include(a => a.Comments)
            //    .ThenInclude(u => u.User)
            //    .FirstOrDefault(a => a.Id == id);

            //if (article == null)
            //{
            //    return NotFound();
            //}

            //if (article.Approved == false)
            //{
            //    article.Approved = true;
            //}
            //else
            //{
            //    article.Approved = false;
            //}
            //db.SaveChanges();

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