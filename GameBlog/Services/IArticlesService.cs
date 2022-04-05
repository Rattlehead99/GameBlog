namespace GameBlog.Services
{
    using GameBlog.Data.Models;
    using GameBlog.Models;
    using Microsoft.AspNetCore.Mvc;

    public interface IArticlesService
    {
        public AllArticlesViewModel GetAllArticles(int pageNumber, string searchText);

        public Task CreateArticle(ArticleViewModel article);

        public ArticleViewModel GetArticleById(Guid id);

        public void EditArticle(ArticleViewModel article);

        public void DeleteArticle([FromForm]Guid id);

        public ArticleViewModel Details(Guid id);

        public Task<Comment> PostComment(CommentViewModel comment);

        public void Approve(Guid id);

    }
}
