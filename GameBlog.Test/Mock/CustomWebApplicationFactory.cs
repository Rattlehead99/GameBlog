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
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {

        public static readonly Guid UserId = Guid.NewGuid();

        protected override void ConfigureClient(HttpClient client)
        {
            base.ConfigureClient(client);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Test");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder
            .ConfigureTestServices(async svc =>
            {
                var descriptor = svc.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<GameBlogDbContext>));

                svc.Remove(descriptor);

                svc.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", x => { });

                svc.AddDbContext<GameBlogDbContext>(options =>
                {
                    options.UseInMemoryDatabase("MemoryDataBase");
                });

                var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
                var claimsPrincipal = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new Claim[] { new Claim(ClaimTypes.NameIdentifier, UserId.ToString()) }
                        )
                    );

                mockHttpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

                descriptor = svc.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(IHttpContextAccessor));
                svc.Remove(descriptor);

                svc.AddSingleton<IHttpContextAccessor>(mockHttpContextAccessor.Object);

                var sp = svc.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<GameBlogDbContext>();
                    var userManager = scopedServices.GetRequiredService<UserManager<User>>();


                    db.Database.EnsureCreated();


                    await userManager.CreateAsync(new User
                    {
                        Id = UserId,
                        UserName = "smth@gmail.com",
                        Email = "smth@gmail.com",
                        NormalizedUserName = "SMTH@GMAIL.COM".Normalize().ToUpperInvariant()
                    });

                    db.Articles.Add(new Article
                    {
                        Id = Guid.NewGuid(),
                        UserId = UserId,
                        Content = "Bulshiser that should reach 30 symbols at the very least",
                        ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
                        Title = "Some dumb title",
                        Approved = false,

                    });
                    db.SaveChanges();
                }
            });
        }
    }
}
