using GameBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlog.Test.TestConstants
{
    public static class TestData
    {
        public static readonly Guid ArticleId = Guid.NewGuid();

        public static ArticleViewModel ArticleView => new ArticleViewModel()
        {
            Content = "Bulshiser that should reach 30 symbols at the very leasfdhhgfcdsvhfsdgsdfgdfsgdsfgsdfgdfst",
            ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
            Title = "Some dumb title123123",
            Approved = false
        };

        public static ArticleViewModel ArticleViewWithId => new ArticleViewModel()
        {
            Id = ArticleId,
            Content = "Bulshiser that should reach 30 symbols at the very leasfdhhgfcdsvhfsdgsdfgdfsgdsfgsdfgdfst",
            ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
            Title = "Some dumb title123123",
            Approved = false
        };
    }
}
