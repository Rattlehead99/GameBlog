using GameBlog.Data;
using GameBlog.Data.Models;

namespace GameBlog.Services
{
    public class PaginationService : IPaginationService
    {
        private readonly GameBlogDbContext db;

        public PaginationService(GameBlogDbContext db)
        {
            this.db = db;
        }
        public int PageCorrection<T>(int pageNumber, IQueryable<T> query) where T : class
        {
            int pageSize = 6;
            double pageCount = Math.Ceiling(query.Count() / (double)pageSize);

            if (pageCount < 1)
            {
                pageCount = 1;
            }

            if (pageNumber < 1)
            {
                pageNumber++;
                pageNumber = 1;
            }
            if (pageNumber > pageCount)
            {
                pageNumber--;
                pageNumber = (int)pageCount;
            }

            return pageNumber;
        }
        public IQueryable<T> Pagination<T>(int pageNumber, IQueryable<T> query) where T : class
        {
            int pageSize = 6;

            var usersQuery = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsQueryable();

            return usersQuery;
        }
       
    }
}
