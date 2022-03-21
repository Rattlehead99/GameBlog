using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameBlog.Data
{
    public class GameBlogDbContext : IdentityDbContext
    {
        public GameBlogDbContext(DbContextOptions<GameBlogDbContext> options)
            : base(options)
        {
        }
    }
}