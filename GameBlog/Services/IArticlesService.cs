namespace GameBlog.Services
{
    using GameBlog.Data.Models;
    using GameBlog.Models;
    using Microsoft.AspNetCore.Mvc;

    public interface IArticlesService
    {
        public AllArticlesViewModel GetAllArticles(int pageNumber, string searchText);

        public Article? CreateArticle(ArticleViewModel article);

        public ArticleViewModel GetArticleById(Guid id);

        public ArticleViewModel EditArticle(ArticleViewModel article);

        public Article DeleteArticle([FromForm]Guid id);

        public ArticleViewModel Details(Guid id);

    }
}
