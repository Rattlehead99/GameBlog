using GameBlog.Data;
using GameBlog.Data.Models;
using GameBlog.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("Server=gameblog.database.windows.net;Database=GameBlog;User Id=rattlehead99;Password=Naviman12#;");

        builder.Services.AddDbContext<GameBlogDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddScoped<IArticlesService, ArticlesService>()
        .AddScoped<IGamesService, GamesService>()
        .AddScoped<IUsersService, UsersService>()
        .AddScoped<IAdministrationService, AdministrationService>()
        .AddScoped<IPaginationService, PaginationService>()
        .AddScoped<IHardwareService, HardwareService>()
        .AddAntiforgery()
        .AddDatabaseDeveloperPageExceptionFilter()
        .AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<GameBlogDbContext>();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+#";
            options.User.RequireUniqueEmail = false;
        });

        builder.Services.AddControllersWithViews();

        WebApplication? app = builder.Build();

        SeedData.InitializeDatabase(app);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}
