

namespace GameBlog.Services
{
    using GameBlog.Data;
    using GameBlog.Data.Models;
    using GameBlog.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesService : IArticlesService
    {
        private readonly GameBlogDbContext db;
        private readonly UserManager<User> userManager;

        public ArticlesService(GameBlogDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public void Approve(Guid id)
        {
            throw new NotImplementedException();
        }

        public Article? CreateArticle(ArticleViewModel article)
        {
            throw new NotImplementedException();
        }

        public Article DeleteArticle([FromForm] Guid id)
        {
            throw new NotImplementedException();
        }

        public ArticleViewModel Details(Guid id)
        {
            throw new NotImplementedException();
        }

        public ArticleViewModel EditArticle(ArticleViewModel article)
        {
            throw new NotImplementedException();
        }

        public AllArticlesViewModel GetAllArticles(int pageNumber=1, string searchText="")
        {
            int pageSize = 6;
            double pageCount = Math.Ceiling(db.Articles.Count() / (double)pageSize);

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

        public ArticleViewModel GetArticleById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void PostComment(CommentViewModel comment)
        {
            throw new NotImplementedException();
        }
    }
}
