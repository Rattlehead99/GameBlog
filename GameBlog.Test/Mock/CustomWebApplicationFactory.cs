using GameBlog.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBlog.Models;
using GameBlog.Data.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Moq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace GameBlog.Test.Mock
{
    public record DependencyScope(AsyncServiceScope Scope, GameBlogDbContext Db)
    {
        public T ResolveService<T>()
            where T : notnull
        {
            return Scope.ServiceProvider.GetRequiredService<T>();
        }
    };

    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {

        public static readonly Guid UserId = Guid.NewGuid();
        public static readonly Guid GameId = Guid.NewGuid();
        public static readonly Guid RatingId = Guid.NewGuid();
        public static readonly Guid HardwareId = Guid.NewGuid();
        private readonly AsyncServiceScope scope;
        protected GameBlogDbContext db;

        protected override void ConfigureClient(HttpClient client)
        {
            base.ConfigureClient(client);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Test");
        }


        public DependencyScope InitDb()
        {
            var scope = Services.CreateAsyncScope();
            db = ResolveService<GameBlogDbContext>();
            Services.AddTestData().GetAwaiter().GetResult();
            return new (scope, db);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder
            .ConfigureTestServices(async svc =>
            {
                svc.RemoveService<DbContextOptions<GameBlogDbContext>>();

                svc.AddAuthentication("Test")
                   .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", x => { });
                
                Guid id = Guid.NewGuid();
                
                svc.AddDbContext<GameBlogDbContext>(options =>
                {
                    options.UseInMemoryDatabase("MemoryDataBase" + id);
                });

                svc.AddTestHttpContextAccessor();

                var sp = svc.BuildServiceProvider();

                //await sp.AddTestData();

            });
        }

        private T ResolveService<T>()
         where T : notnull
        {
            return this.Services.CreateAsyncScope().ServiceProvider.GetRequiredService<T>();
        }


    }
}
