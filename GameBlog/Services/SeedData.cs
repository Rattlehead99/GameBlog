

namespace GameBlog.Services
{
    using GameBlog.Data;
    using GameBlog.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using static GameBlog.Data.Models.User;
    using Microsoft.Extensions.DependencyInjection;

    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider app)
        {
            var roleManger = app
                .GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            await EnsureRoleAsync(roleManger);

            var userManger = app
                .GetRequiredService<UserManager<User>>();

            await EnsureTestAdminAsync(userManger);
        }

        private static async Task EnsureRoleAsync(RoleManager<IdentityRole<Guid>> roleManger)
        {
            var alreadyExists = await roleManger
                .RoleExistsAsync(DataConstants.Role.Administrator);

            if (alreadyExists)
            {
                return;
            }

            await roleManger.CreateAsync(new IdentityRole<Guid>(DataConstants.Role.Administrator));
        }

        private static async Task EnsureTestAdminAsync(UserManager<User> userManager)
        {
            var testAdmin = await userManager.Users
                .Where(x => x.UserName == "ikovachev99@gmail.com")
                .SingleOrDefaultAsync();

            if (testAdmin == null)
            {
                return;
            }

            //testAdmin = new User
            //{
            //    UserName = "ikovachev99@gmail.com",
            //    Email = "ikovachev99@gmail.com"
            //};

            await userManager.AddToRoleAsync(testAdmin, DataConstants.Role.Administrator);
        }

        public static void InitializeDatabase(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    SeedData.InitializeAsync(services).Wait();
                }
                catch (Exception ex)
                {
                    ILogger<Program>? logger = services
                        .GetRequiredService<ILogger<Program>>();
                    
                    logger.LogError(ex, "Error occurred seeding the DB.");
                }
            }
        }
    }
}
