﻿namespace GameBlog.Models
{
    public class AllArticlesViewModel
    {
        public IEnumerable<ArticleViewModel> Articles { get; set; } = new List<ArticleViewModel>();

        public int PageNumber { get; set; }
    }
}
