

namespace GameBlog.Services
{
    using GameBlog.Data;
    using GameBlog.Data.Models;
    using GameBlog.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.EntityFrameworkCore;
    using static GameBlog.Data.DataConstants.Role;

    public class ArticlesService : IArticlesService
    {
        private readonly GameBlogDbContext db;
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        public ArticlesService(GameBlogDbContext db, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.db = db;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public void Approve(Guid id)
        {
            var article = db.Articles
              .Include(a => a.Comments)
              .ThenInclude(u => u.User)
              .FirstOrDefault(a => a.Id == id);

            if (article == null)
            {
                throw new ArgumentNullException("Article not found");
            }

            if (article.Approved == false)
            {
                article.Approved = true;
            }
            else
            {
                article.Approved = false;
            }
            db.SaveChanges();
        }
            
        public async Task CreateArticle(ArticleViewModel article)
        {
            ClaimsPrincipal? userContext = httpContextAccessor.HttpContext?.User;
            User? user = await userManager.GetUserAsync(userContext);

            Article? articleData = new Article
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
        }

        public void DeleteArticle([FromForm] Guid id)
        {
            Article? articleData = db.Articles.Find(id);

            if (articleData == null)
            {
                throw new ArgumentNullException("Article not found");
            }

            db.Articles.Remove(articleData);
            db.SaveChanges();
        }

       

        public void EditArticle(ArticleViewModel article)
        {
            var articleData = db.Articles.Find(article.Id);
            if (articleData == null)
            {
                throw new ArgumentNullException("Article not found");
            }
            articleData.Approved = article.Approved;
            articleData.Comments = article.Comments;
            articleData.Content = article.Content;
            articleData.ImageUrl = article.ImageUrl;
            articleData.Title = article.Title;

            db.Articles.Update(articleData);
            db.SaveChanges();
        }

        public AllArticlesViewModel GetAllArticles(int pageNumber=1, string searchText="")
        {
            int pageSize = 6;
            double pageCount = Math.Ceiling(db.Articles.Count() / (double)pageSize);

            if (pageNumber < 1)
            {
                pageNumber++;
            }
            if (pageNumber > pageCount)
            {
                pageNumber--;
            }

            var articlesQuery = db.Articles
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(a => a.PostDate).AsQueryable();

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

            return new AllArticlesViewModel
            {
                Articles = articles,
                PageNumber = pageNumber
                
            };
        }
        public ArticleViewModel Details(Guid id)
        {
            var article = db.Articles
             .Include(a => a.Comments)
             .ThenInclude(u => u.User)
             .FirstOrDefault(a => a.Id == id);

            if (article == null)
            {
                throw new ArgumentNullException("Article not found");
            }

            var articleView = new ArticleViewModel
            {
                Approved = article.Approved,
                Comments = article.Comments.OrderByDescending(c => c.PostDate).ToList(),
                Content = article.Content,
                Id = id,
                ImageUrl = article.ImageUrl,
                Title = article.Title,
                UserId = id
            };

            return articleView;
        }

        public ArticleViewModel GetArticleById(Guid id)
        {
            Article? article = db.Articles.SingleOrDefault(a => a.Id == id);
            
            if (article == null)
            {
                throw new ArgumentNullException("Article can not be found");
            }

            var articleView = new ArticleViewModel
            {
                Approved = article.Approved,
                Comments = article.Comments,
                Content = article.Content,
                Id = id,
                ImageUrl = article.ImageUrl,
                Title = article.Title,
            };
            return articleView;
        }

        public async Task<Comment> PostComment(CommentViewModel comment)
        {
            ClaimsPrincipal? userContext = httpContextAccessor.HttpContext?.User;
            User? user = await userManager.GetUserAsync(userContext);

            bool articleData = db.Articles
               .Any(a => a.Id == comment.ArticleId);

            if (!articleData)
            {
                throw new ArgumentNullException("Article not found");
            }

            Comment? commentData = new Comment
            {
                Id = Guid.NewGuid(),
                ArticleId =comment.ArticleId,
                UserId = user.Id,
                Content = comment.Content
            };

            db.Comments.Add(commentData);
            db.SaveChanges();

            return commentData;
        }
    }
}
