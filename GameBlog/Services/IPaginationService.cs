using GameBlog.Data.Models;

namespace GameBlog.Services
{
    public interface IPaginationService
    {
        public IQueryable<T> Pagination<T>(int pageNumber, IQueryable<T> query) where T : class;

        public int PageCorrection<T>(int pageNumber, IQueryable<T> query) where T : class;
    }
}
