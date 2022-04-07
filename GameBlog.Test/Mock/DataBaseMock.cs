namespace GameBlog.Test.Mock
{
    using GameBlog.Data;
    using Microsoft.EntityFrameworkCore;
    using System;

    public static class DataBaseMock
    {
        public static GameBlogDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<GameBlogDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new GameBlogDbContext(dbContextOptions);
            }
        }
    }
}
