using GameBlog.Data;
using GameBlog.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GameBlog.Test.Mock
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RemoveService<TService>(this IServiceCollection svc)
        {
            var descriptor = svc.SingleOrDefault(
              d => d.ServiceType ==
                  typeof(TService)) ?? throw new ArgumentException(nameof(TService));

            svc.Remove(descriptor);

            return svc;
        }

        public static IServiceCollection AddTestHttpContextAccessor(this IServiceCollection svc)
        {
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var claimsPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[] { new Claim(ClaimTypes.NameIdentifier, CustomWebApplicationFactory.UserId.ToString()) }
                    )
                );

            mockHttpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

            svc.RemoveService<IHttpContextAccessor>();

            svc.AddSingleton<IHttpContextAccessor>(mockHttpContextAccessor.Object);

            return svc;
        }

        public static async Task<IServiceProvider> AddTestData(this IServiceProvider serviceProvider)
        {

            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<GameBlogDbContext>();
                var userManager = scopedServices.GetRequiredService<UserManager<User>>();
                var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

                //https://github.com/dotnet/efcore/issues/6282#issuecomment-509684621
                db.ChangeTracker.Clear();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                await userManager.CreateAsync(new User
                {
                    Id = CustomWebApplicationFactory.UserId,
                    UserName = "smth@gmail.com",
                    Email = "smth@gmail.com",
                    NormalizedUserName = "SMTH@GMAIL.COM".Normalize().ToUpperInvariant()
                });

                db.Articles.Add(new Article
                {
                    Id = Guid.NewGuid(),
                    UserId = CustomWebApplicationFactory.UserId,
                    Content = "Bulshiser that should reach 30 symbols at the very least",
                    ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
                    Title = "Some dumb title",
                    Approved = false,

                });

                db.Games.Add(new Game
                {
                    Description = "Something that is 30 symbols long I think",
                    Ratings = new List<Rating>()
                        {
                            new Rating()
                            {
                                GameId = CustomWebApplicationFactory.GameId,
                                Id = CustomWebApplicationFactory.RatingId,
                                RatingValue = 10,
                                UserId = CustomWebApplicationFactory.UserId
                            }
                        },
                    Id = CustomWebApplicationFactory.GameId,
                    Genre = "Action-Adventure",
                    ImageUrl = "https://media.wired.com/photos/5b899992404e112d2df1e94e/master/pass/trash2-01.jpg",
                    Name = "Super Mario Bros"
                });

                var roleName = "Administrator";

                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));

                db.SaveChanges();

                return serviceProvider;
            }
        }
    }

}

